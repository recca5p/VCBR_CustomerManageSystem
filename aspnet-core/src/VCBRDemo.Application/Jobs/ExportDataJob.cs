using Microsoft.AspNetCore.Authorization;
using Quartz;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers.DTOs;
using VCBRDemo.Customers.Interfaces;
using VCBRDemo.ExportRequests;
using VCBRDemo.ExportRequests.DTOs;
using VCBRDemo.ExportRequests.Interfaces;
using VCBRDemo.Files.Interfaces;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BackgroundWorkers.Quartz;

namespace VCBRDemo.Jobs
{
    [DisallowConcurrentExecution]
    public class ExportDataJob: QuartzBackgroundWorkerBase
    {
        private readonly IExportRequestAppService _exportRequestAppService;
        private readonly ICustomerAppService _customerAppSerivce;
        private readonly IFileAppService _fileAppSerivce;
        public ExportDataJob(IExportRequestAppService exportRequestAppService,
            ICustomerAppService customerAppService,
            IFileAppService fileAppService)
        {
            _exportRequestAppService = exportRequestAppService;
            _customerAppSerivce = customerAppService;
            JobDetail = JobBuilder.Create<ImportDataJob>().WithIdentity("AbpIdentity.Users").Build();
            Trigger = TriggerBuilder.Create().WithIdentity("AbpIdentity.Users").StartNow().Build();
            _fileAppSerivce = fileAppService;
        }
        public override async Task Execute(IJobExecutionContext context)
        {

            //Get the earlist request
            ExportRequestDTO exportRequest = await _exportRequestAppService.GetEarliestExportRequestAsync();
            if (exportRequest == null)
            {
                return;
            }
            try
            {
                //Begin process
                await _exportRequestAppService.UpdateExportRequestAsync(exportRequest.Id, ExportRequestStatusEnum.Created, null, null);

                //Get list customer base on the request filter
                CustomerFilterListDTO filter = new CustomerFilterListDTO
                {
                    Filter = exportRequest.Filter,
                    SkipCount = 0,
                    MaxResultCount = 1000,
                    Sorting = string.Empty
                };
                PagedResultDto<CustomerDTO> customers = await _customerAppSerivce.GetListForWorkerAsync(filter);

                if(customers.Items.Count == 0)
                {
                    await _exportRequestAppService.UpdateExportRequestAsync(exportRequest.Id, ExportRequestStatusEnum.Failed, null, "No data found");
                    return;
                }

                List<CustomerDTO> tempList = new List<CustomerDTO>();
                foreach(var c in customers.Items)
                {
                    tempList.Add(c);
                }

                byte[] excelData =await _exportRequestAppService.ExportDataToExcel(tempList);

                DateTime now = DateTime.Now;
                long ticks = now.Ticks;

                string key = $"{now.ToString("yyyyMMddHHmmssfff")}{ticks.ToString().Trim()}_Import_Report";
                key = key.Replace(" ", ""); // Remove any spaces

                _fileAppSerivce.UploadFileAsync(excelData, key, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                await _exportRequestAppService.UpdateExportRequestAsync(exportRequest.Id, ExportRequestStatusEnum.Success, key, "Export success");

                return;
            }
            catch (Exception ex) 
            {
                await _exportRequestAppService.UpdateExportRequestAsync(exportRequest.Id, ExportRequestStatusEnum.Failed, null, ex.Message);
                throw;
            }
        }
    }
}
