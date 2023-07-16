using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers;
using VCBRDemo.Customers.Interfaces;
using VCBRDemo.Files.DTOs;
using VCBRDemo.Files.Interfaces;
using VCBRDemo.Permissions;

namespace VCBRDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : VCBRDemoController
    {
        private readonly IFileAppService _fileAppService;
        public FileController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [Authorize(VCBRDemoPermissions.Customers.ImportFile)]
        [HttpPost]
        public async Task<IActionResult> ImportFile([FromForm] UploadFileRequestDTO model)
        {
            try
            {
                //check if file is uploaded or not
                if (model.File == null || model.File.Length <= 0)
                {
                    return new JsonResult(
                        new
                        {
                            Id = -1,
                            Message = "No attachment uploaded"
                        });
                }

                var result = await _fileAppService.UploadFileAsync(model.File);

                if (result.Success)
                {
                    return new JsonResult(new
                    {
                        Id = 1,
                        Message = "Upload files successfully"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("download/{key}")]
        public async Task<IActionResult> Download(string key)
        {
            var result = await _fileAppService.DownloadFileAsync(key);
            if (result.Success)
            {
                return File(result.FileStream, result.ContentType, key);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
            }
        }
    }
}
