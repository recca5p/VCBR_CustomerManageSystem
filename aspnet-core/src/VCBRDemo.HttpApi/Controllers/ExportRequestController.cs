using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.ExportRequests.DTOs;
using VCBRDemo.ExportRequests.Interfaces;

namespace VCBRDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportRequestController : VCBRDemoController
    {
        private readonly IExportRequestAppService _exportRequestAppService;

        public ExportRequestController(IExportRequestAppService exportRequestAppService) 
        {
            _exportRequestAppService = exportRequestAppService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExportRequest (ExportRequestCreateDTO model)
        {
            var res = await _exportRequestAppService.CreateExportRequestAsync(model);
            return new JsonResult(res);
        }
    }
}
