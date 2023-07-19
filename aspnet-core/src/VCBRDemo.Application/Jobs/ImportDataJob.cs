
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers.Interfaces;
using VCBRDemo.Files;
using VCBRDemo.Files.DTOs;
using VCBRDemo.Files.Interfaces;
using VCBRDemo.ImportRequests;
using VCBRDemo.ImportRequests.DTOs;
using VCBRDemo.ImportRequests.Interfaces;
using VCBRDemo.Permissions;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Http;
using Volo.Abp.Uow;

namespace VCBRDemo.Jobs
{
    [DisallowConcurrentExecution]
    public class ImportDataJob : QuartzBackgroundWorkerBase
    {
        private readonly IImportRequestAppService _importRequestAppService;
        private readonly IFileAppService _fileAppService;
        private readonly ICustomerAppService _customerAppService;
        private readonly IHubContext<ImportRequestHub> _hubContext;
        public ImportDataJob(IImportRequestAppService importRequestAppService,
            IFileAppService fileAppService,
            ICustomerAppService customerAppService,
            IHubContext<ImportRequestHub> hubContext)
        {
            _importRequestAppService = importRequestAppService;
            _fileAppService = fileAppService;
            JobDetail = JobBuilder.Create<ImportDataJob>().WithIdentity("AbpIdentity.Users").Build();
            Trigger = TriggerBuilder.Create().WithIdentity("AbpIdentity.Users").StartNow().Build();
            _customerAppService = customerAppService;
            _hubContext = hubContext;
        }
        public override async Task Execute(IJobExecutionContext context)
        {
            ImportRequestDTO importRequest = await _importRequestAppService.GetEarliestImportrequestAsync();

            if (importRequest == null)
                return;
            try
            {
                await _hubContext.Clients.All.SendAsync("Import status updated", importRequest);

                //update status to begin process
                ImportRequestResponseDTO beginProcess = await _importRequestAppService.UpdateImportRequestAsync(importRequest.Id, ImportRequestStatusEnum.Executing, null, null);

                //download from s3
                DownloadFileResponseDTO file = await _fileAppService.DownloadFileAsync(importRequest.FileId);
                // Convert the file stream to a byte array
                byte[] fileBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.FileStream.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                ImportDataIntoDatabaseDTO result = await _importRequestAppService.ImportDataIntoDatabaseAsync(fileBytes);

                DateTime now = DateTime.Now;
                long ticks = now.Ticks;

                string key = $"{now.ToString("yyyyMMddHHmmssfff")}{ticks.ToString().Trim()}_Import_Report";
                key = key.Replace(" ", ""); // Remove any spaces
                await _fileAppService.UploadFileAsync(result.fileBytes, key, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                //Assign customer to user role
                _customerAppService.AddCustomerToUserRole(result.userIds);

                //update status to sucesss
                ImportRequestResponseDTO successProcess = await _importRequestAppService.UpdateImportRequestAsync(importRequest.Id, ImportRequestStatusEnum.Success, key, "Import success, please view report file for more detail");
                //delete the source report on s3
                await _fileAppService.DeleteFileAsync(importRequest.FileId);
                return;
        }
            catch (Exception ex)
            {
                //update status to failed
                ImportRequestResponseDTO failProcess = await _importRequestAppService.UpdateImportRequestAsync(importRequest.Id, ImportRequestStatusEnum.Failed, null, ex.Message);
            }
}
    }
}
