using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.ExportRequests.DTOs;
using VCBRDemo.ExportRequests.Interfaces;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace VCBRDemo.ExportRequests
{
    [RemoteService(IsEnabled = false)]
    public class ExportRequestAppService : VCBRDemoAppService, IExportRequestAppService
    {
        private readonly IExportRequestRepository _exportRequestRepository;
        public ExportRequestAppService(IExportRequestRepository exportRequestRepository) 
        {
            _exportRequestRepository = exportRequestRepository;
        }

        public async Task<ExportRequestCreateDTO> CreateExportRequestAsync(ExportRequestCreateDTO request)
        {
            var res = await _exportRequestRepository.GetListAsync();
            return null;
        }
    }
}
