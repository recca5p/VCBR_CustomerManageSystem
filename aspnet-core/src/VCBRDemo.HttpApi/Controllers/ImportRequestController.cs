using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
        [Authorize(Permissions.VCBRDemoPermissions.Customers.ImportFile)]
        public async Task<IActionResult> ImportCustomersByFileAsync([FromForm] ImportRequestCreateDTO model)
        {
            try
            {
                // Check if a file is provided
                var file = model.File;
                if (file == null || file.Length <= 0)
                {
                    return new BadRequestObjectResult("No file provided.");
                }

                // Ensure the file has valid extensions
                var allowedExtensions = new[] { ".xls", ".xlsx", ".csv" };
                var fileExtension = Path.GetExtension(file.FileName);
                if (!allowedExtensions.Contains(fileExtension.ToLower()))
                {
                    return new BadRequestObjectResult("Invalid file extension. Only Excel, CSV, and text files are allowed.");
                }

                ImportRequestResponseDTO result = await _importRequestAppService.ImportCustomersByFileAsync(model);

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
    }
}
