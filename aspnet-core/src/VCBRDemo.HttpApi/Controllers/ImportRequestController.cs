using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers.Interfaces;
using VCBRDemo.Files.Interfaces;
using VCBRDemo.ImportRequests.DTOs;
using VCBRDemo.ImportRequests.Interfaces;

namespace VCBRDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportRequestController : VCBRDemoController
    {
        private readonly IImportRequestAppService _importRequestAppService;
        private readonly IFileAppService _fileAppService;

        public ImportRequestController(IImportRequestAppService importRequestAppService,
            IFileAppService fileAppService)
        {
            _importRequestAppService = importRequestAppService;
            _fileAppService = fileAppService;
        }

        [HttpPost]
        public async Task<IActionResult> ImportCustomersByFileAsync([FromForm] ImportRequestCreateDTO model)
        {
            try
            {
                var result = await _importRequestAppService.ImportCustomersByFileAsync(model);
                if (result.Id == 1)
                {
                    return new JsonResult(result);
                }
                else
                {
                    return new BadRequestObjectResult("Create request failed");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpGet("key")]
        public async Task<IActionResult> DownloadS3File(string key)
        {
            var result = await _fileAppService.DownloadFileAsync(key);

            return File(result.FileStream, result.ContentType, key);
        }
    }
}
