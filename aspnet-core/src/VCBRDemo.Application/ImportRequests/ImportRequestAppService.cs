using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Files.DTOs;
using VCBRDemo.Files.Interfaces;
using VCBRDemo.ImportRequests.DTOs;
using VCBRDemo.ImportRequests.Interfaces;
using Volo.Abp;

namespace VCBRDemo.ImportRequests
{
    [RemoteService(IsEnabled = false)]
    public class ImportRequestAppService : VCBRDemoAppService, IImportRequestAppService
    {
        private readonly IImportRequestRepository _importRequestRepository;
        private readonly IFileAppService _fileAppService;

        public ImportRequestAppService(IImportRequestRepository importRequestRepository, IFileAppService fileAppService)
        {
            _importRequestRepository = importRequestRepository;
            _fileAppService = fileAppService;
        }

        public async Task<ImportRequestDTO> ImportCustomersByFileAsync(ImportRequestCreateDTO file)
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
                /*Try to upload file requested to s3*/
                UploadFileResponseDTO result = await _fileAppService.UploadFileAsync(file.File, key);
                if (result.Success)
                {
                    return new ImportRequestDTO
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
                    return new ImportRequestDTO
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
    }
}
