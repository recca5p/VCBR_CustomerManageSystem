using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.ExportRequests.DTOs;
using Volo.Abp.Application.Services;

namespace VCBRDemo.ExportRequests.Interfaces
{
    public interface IExportRequestAppService : IApplicationService
    {
        Task<ExportRequestCreateDTO> CreateExportRequestAsync(ExportRequestCreateDTO request);
    }
}
