using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Files.Interfaces;

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

        [HttpGet("key")]
        public async Task<IActionResult> DownloadS3File(string key)
        {
            var result = await _fileAppService.DownloadFileAsync(key);

            return File(result.FileStream, result.ContentType, key);
        }
    }
}
