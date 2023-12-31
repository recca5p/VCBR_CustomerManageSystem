﻿using Abp.Application.Services.Dto;
using Aspose.Cells;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers.DTOs;
using VCBRDemo.ExportRequests.DTOs;
using VCBRDemo.ExportRequests.Interfaces;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Uow;

namespace VCBRDemo.ExportRequests
{
    public class ExportRequestAppService : VCBRDemoAppService, IExportRequestAppService
    {
        private readonly IExportRequestRepository _exportRequestRepository;
        private readonly IHubContext<ExportRequestHub> _hubContext;
        public ExportRequestAppService(IExportRequestRepository exportRequestRepository, IHubContext<ExportRequestHub> hubContext) 
        {
            _exportRequestRepository = exportRequestRepository;
            _hubContext = hubContext;
        }

        [Authorize(Permissions.VCBRDemoPermissions.Customers.ExportFile)]
        public async Task<PagedResultDto<ExportRequestCrudDTO>> GetList (PagedAndSortedResultRequestDto model)
        {
            List<ExportRequest> list = await  _exportRequestRepository.GetListAsync();

            List<ExportRequestCrudDTO> res = ObjectMapper.Map<List<ExportRequest>, List<ExportRequestCrudDTO>>(list);

            PagedResultDto<ExportRequestCrudDTO> result = new PagedResultDto<ExportRequestCrudDTO>();
            int totalCount = res.Count();
            result.TotalCount = totalCount;
            result.Items = res.Skip(model.SkipCount).OrderByDescending(_ => _.CreationTime).Take(model.MaxResultCount).ToList();
            

            return result;
        }

        [RemoteService(IsEnabled = false)]
        [Authorize(Permissions.VCBRDemoPermissions.Customers.ExportFile)]
        public async Task<ExportRequestReturnDTO> CreateExportRequestAsync(ExportRequestCreateDTO request)
        {
            try
            {
                ExportRequest paramObj = new ExportRequest
                {
                    Filter = request.Filter.ToString(),
                    FileId = null,
                    RequestStatus = ExportRequestStatusEnum.Created,
                    Result = null
                };
                ExportRequest result = await _exportRequestRepository.InsertAsync(paramObj);
                ExportRequestCrudDTO returnDTO = new ExportRequestCrudDTO();
                returnDTO = ObjectMapper.Map<ExportRequest, ExportRequestCrudDTO>(result);
                await _hubContext.Clients.All.SendAsync("added", returnDTO);

                return new ExportRequestReturnDTO
                {
                    Id = 1,
                    Message = "Create request success"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [RemoteService(IsEnabled = false)]
        public async Task<ExportRequestDTO> GetEarliestExportRequestAsync()
        {
            ExportRequest exReq =await _exportRequestRepository.GetEarliestExportRequestAsync();
            ExportRequestDTO result = ObjectMapper.Map<ExportRequest, ExportRequestDTO>(exReq);
            return result;
        }

        [RemoteService(IsEnabled = false)]
        public async Task<ExportRequestReturnDTO> UpdateExportRequestAsync(Guid importRequestId, ExportRequestStatusEnum status, string fileId, string result)
        {
            ExportRequest exportRequest = await _exportRequestRepository.GetAsync(importRequestId);

            exportRequest.RequestStatus = status;

            if(!fileId.IsNullOrEmpty())
            {
                exportRequest.FileId = fileId;
            }

            if(!result.IsNullOrEmpty())
            {
                exportRequest.Result = result;
            }

            await _exportRequestRepository.UpdateAsync(exportRequest);
            ExportRequest exportRequest2 = await _exportRequestRepository.GetAsync(importRequestId);
            ExportRequestCrudDTO result2 = ObjectMapper.Map<ExportRequest, ExportRequestCrudDTO>(exportRequest2);

            await _hubContext.Clients.All.SendAsync("update", result2);

            return new ExportRequestReturnDTO
            {
                Id = 1,
                Message = "Update success"
            };
        }

        [RemoteService(IsEnabled = false)]
        public async Task<byte[]> ExportDataToExcel (List<CustomerDTO> customers)
        {
            try
            {
                // Create the Excel workbook
                Workbook workbook = new Workbook();

                // Create the worksheets for result list and failure list
                Worksheet resultSheet = workbook.Worksheets.Add("Customers");

                // Fill the result list sheet with the data from the result list
                FillWorksheetWithData(resultSheet, customers);

                // Convert the workbook to a byte array
                byte[] excelData;
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.Save(stream, SaveFormat.Xlsx);
                    excelData = stream.ToArray();
                }

                return excelData;
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
    }
}
