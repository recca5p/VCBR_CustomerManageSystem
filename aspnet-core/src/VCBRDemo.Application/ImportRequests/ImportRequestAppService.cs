using Aspose.Cells;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers;
using VCBRDemo.Customers.DTOs;
using VCBRDemo.Customers.Interfaces;
using VCBRDemo.Files;
using VCBRDemo.Files.DTOs;
using VCBRDemo.Files.Interfaces;
using VCBRDemo.ImportRequests.DTOs;
using VCBRDemo.ImportRequests.Interfaces;
using Volo.Abp;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using ZstdSharp.Unsafe;

namespace VCBRDemo.ImportRequests
{
    [RemoteService(IsEnabled = false)]
    public class ImportRequestAppService : VCBRDemoAppService, IImportRequestAppService
    {
        private readonly IImportRequestRepository _importRequestRepository;
        private readonly IFileAppService _fileAppService;
        private readonly ICustomerAppService _customerAppService;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly ICustomerRepository _customerRepository;

        public ImportRequestAppService(IImportRequestRepository importRequestRepository,
            IFileAppService fileAppService,
            ICustomerAppService customerAppService,
            IIdentityUserAppService identityUserAppService,
            IIdentityUserRepository identityUserRepository,
            ICustomerRepository customerRepository)
        {
            _importRequestRepository = importRequestRepository;
            _fileAppService = fileAppService;
            _customerAppService = customerAppService;
            _identityUserRepository = identityUserRepository;
            _customerRepository = customerRepository;
        }

        public async Task<ImportRequestResponseDTO> ImportCustomersByFileAsync(ImportRequestCreateDTO file)
        {
            try
            {
                DateTime now = DateTime.Now;
                long ticks = now.Ticks;

                string key = $"{now.ToString("yyyyMMddHHmmssfff")}{ticks.ToString().Trim()}{file.File.FileName}";
                key = key.Replace(" ", ""); // Remove any spaces
                /*Create a request in DB */
                ImportRequest importRequest = new ImportRequest
                {
                    Filename = file.File.FileName,
                    Extension = Path.GetExtension(file.File.FileName),
                    FileId = key,
                    RequestStatus = Files.ImportRequestStatusEnum.Created,
                    Result = string.Empty,
                    ReportId = string.Empty,
                };

                ImportRequest createRes = await _importRequestRepository.InsertAsync(importRequest);

                byte[] fileBytes;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.File.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }                /*Try to upload file requested to s3*/
                UploadFileResponseDTO result = await _fileAppService.UploadFileAsync(fileBytes, key, file.File.ContentType);
                if (result.Success)
                {
                    return new ImportRequestResponseDTO
                    {
                        Id = 1,
                        Message = "Create success"
                    };
                }
                else
                {
                    createRes.RequestStatus = Files.ImportRequestStatusEnum.Failed;
                    createRes.Result = "Upload on S3 failed";
                    await _importRequestRepository.UpdateAsync(createRes);
                    return new ImportRequestResponseDTO
                    {
                        Id = -1,
                        Message = "Upload on S3 failed"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ImportRequestDTO> GetEarliestImportrequestAsync()
        {
            ImportRequest importRequest = await _importRequestRepository.GetEarliestImportrequestAsync();

            ImportRequestDTO importRequestDTO = ObjectMapper.Map<ImportRequest, ImportRequestDTO>(importRequest);

            return importRequestDTO;
        }

        public async Task<ImportRequestResponseDTO> UpdateImportRequestAsync(Guid importRequestId, ImportRequestStatusEnum status, string reportId, string result)
        {
            ImportRequest importRequest = await _importRequestRepository.GetAsync(importRequestId);

            importRequest.RequestStatus = status;
            if(!reportId.IsNullOrEmpty())
            {
                importRequest.ReportId = reportId;
            }

            if(!result.IsNullOrEmpty())
            {
                importRequest.Result = result;
            }

            await _importRequestRepository.UpdateAsync(importRequest);

            return new ImportRequestResponseDTO
            {
                Id = 1,
                Message = "Update status success"
            };
        }

        public async Task<ImportDataIntoDatabaseDTO> ImportDataIntoDatabaseAsync(byte[] fileArray)
        {
            try
            {
                Stream file = new MemoryStream(fileArray);

                if (!ValidateExcelTemplate(file))
                {
                    throw new Exception("Invalid Excel template.");
                }
                var (successList, failureList) = ReadExcelData(file);

                List<CustomerDTO> result = new List<CustomerDTO>();

                foreach (CustomerInsertDTO entity in successList)
                {
                    //Check is the identitynumber exist, if yes insert into failure list
                    CustomerDTO customerExisting = await _customerAppService.FindByIdentityNumberAsync(entity.IdentityNumber);
                    IdentityUser emailExisting = await _identityUserRepository.FindByNormalizedEmailAsync(entity.Email.ToUpper());

                    if (customerExisting != null || emailExisting != null)
                    {
                        entity.ImportStatus = ImportRequestStatusEnum.Failed;
                        entity.Message = "Customer existings, please check email or identity number";
                        failureList.Add(entity);
                        continue;
                    }
                    CustomerCreateDTO createCustomer = new CustomerCreateDTO
                    {
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Gender = (CustomerGenderEnum)entity.Gender,
                        Email = entity.Email,
                        Address = entity.Address,
                        PhoneNumber = entity.PhoneNumber,
                        IdentityNumber = entity.IdentityNumber,
                        Balance = entity.Balance,
                        Password = $"{entity.IdentityNumber}Customer*",
                        // Map other properties accordingly
                    };

                    CustomerDTO res = await _customerAppService.CreateUsingByWorkerAsync(createCustomer);
                    result.Add(res);
                }
                // Create the Excel workbook
                Workbook workbook = new Workbook();

                // Create the worksheets for result list and failure list
                Worksheet resultSheet = workbook.Worksheets.Add("Result");
                Worksheet failureSheet = workbook.Worksheets.Add("Failure");

                // Fill the result list sheet with the data from the result list
                FillWorksheetWithData(resultSheet, result);

                // Fill the failure list sheet with the data from the failure list
                FillWorksheetWithData(failureSheet, failureList);

                // Convert the workbook to a byte array
                byte[] excelData;
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.Save(stream, SaveFormat.Xlsx);
                    excelData = stream.ToArray();
                }
                ImportDataIntoDatabaseDTO returnResult = new ImportDataIntoDatabaseDTO
                {
                    fileBytes = excelData,
                    userIds = result.Select(_ => _.UserId).ToList()
                };

                return returnResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void FillWorksheetWithData<T>(Worksheet worksheet, List<T> dataList)
        {
            if (dataList.Count > 0)
            {
                Cells cells = worksheet.Cells;
                DataTable dataTable = ListToDataTable(dataList);
                cells.ImportDataTable(dataTable, true, 0, 0);
            }
        }

        private DataTable ListToDataTable<T>(List<T> list)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in list)
            {
                DataRow row = dataTable.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private (List<CustomerInsertDTO> successList, List<CustomerInsertDTO> failureList) ReadExcelData(Stream fileStream)
        {
            List<CustomerInsertDTO> successList = new List<CustomerInsertDTO>();
            List<CustomerInsertDTO> failureList = new List<CustomerInsertDTO>();

            // Load the Excel file using Aspose.Cells
            Workbook workbook = new Workbook(fileStream);

            // Access the first worksheet in the workbook
            Worksheet worksheet = workbook.Worksheets[0];

            // Iterate over the rows and populate the customers list
            for (int row = 1; row <= worksheet.Cells.MaxDataRow; row++)
            {
                CustomerInsertDTO customer = new CustomerInsertDTO();

                customer.FirstName = worksheet.Cells[row, 0].StringValue;
                customer.LastName = worksheet.Cells[row, 1].StringValue;

                if (Enum.TryParse<CustomerGenderEnum>(worksheet.Cells[row, 2].StringValue, out CustomerGenderEnum gender))
                {
                    customer.Gender = gender;
                }
                else
                {
                    // Handle the parsing error gracefully
                    // Override the cell value with the original value
                    customer.Gender = null;
                    worksheet.Cells[row, 2].PutValue("###<" + worksheet.Cells[row, 2].StringValue + "###");
                    customer.ImportStatus = ImportRequestStatusEnum.Failed;
                    customer.Message = "Parsing failed";
                    failureList.Add(customer);
                    continue;
                }

                customer.Email = worksheet.Cells[row, 3].StringValue;
                customer.Address = worksheet.Cells[row, 4].StringValue;
                customer.PhoneNumber = worksheet.Cells[row, 5].StringValue;
                customer.IdentityNumber = worksheet.Cells[row, 6].StringValue;

                if (double.TryParse(worksheet.Cells[row, 7].StringValue, out double balance))
                {
                    customer.Balance = balance;
                }
                else
                {
                    // Handle the parsing error gracefully
                    // Override the cell value with the original value
                    customer.Balance = null;
                    worksheet.Cells[row, 7].PutValue("###<" + worksheet.Cells[row, 7].StringValue + "###");
                    customer.ImportStatus = ImportRequestStatusEnum.Failed;
                    customer.Message = "Parsing failed";
                    failureList.Add(customer);
                    continue;
                }

                // Set the default Status and Message for successful parsing
                customer.ImportStatus = ImportRequestStatusEnum.Created;
                customer.Message = "Insert successful";

                // Add the customer to the successList
                successList.Add(customer);
            }

            // Dispose the workbook object
            workbook.Dispose();

            return (successList, failureList);
        }

        private bool ValidateExcelTemplate(Stream fileStream)
        {
            // Load the Excel file using Aspose.Cells
            Workbook workbook = new Workbook(fileStream);

            // Access the first worksheet in the workbook
            Worksheet worksheet = workbook.Worksheets[0];

            // Read the header row from the worksheet
            Row headerRow = worksheet.Cells.Rows[0];

            // Get the column names from the Customer model using reflection
            var propertiesToCheck = new List<string>
            {
                nameof(Customer.FirstName),
                nameof(Customer.LastName),
                nameof(Customer.Gender),
                nameof(Customer.Email),
                nameof(Customer.Address),
                nameof(Customer.PhoneNumber),
                nameof(Customer.IdentityNumber),
                nameof(Customer.Balance)
                // Add other properties to check as needed
            };
            // Get the column names from the Customer model's properties to check
            var expectedColumnNames = propertiesToCheck;

            // Compare the column names in the Excel file with the expected column names
            var actualColumnNames = new List<string>();
            for (int column = 0; column <= headerRow.LastCell.Column; column++)
            {
                Cell cell = headerRow.GetCellOrNull(column);
                if (cell != null)
                {
                    string columnName = cell.StringValue;
                    actualColumnNames.Add(columnName);
                }
            }

            bool isValidTemplate = expectedColumnNames.SequenceEqual(actualColumnNames);

            // Dispose the workbook object
            workbook.Dispose();

            return isValidTemplate;
        }
    }
}
