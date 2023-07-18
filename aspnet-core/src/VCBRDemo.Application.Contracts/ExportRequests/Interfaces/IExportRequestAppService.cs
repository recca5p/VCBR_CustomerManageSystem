using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers.DTOs;
using VCBRDemo.ExportRequests.DTOs;
using Volo.Abp.Application.Services;

namespace VCBRDemo.ExportRequests.Interfaces
{
    public interface IExportRequestAppService : IApplicationService
    {
        Task<ExportRequestReturnDTO> CreateExportRequestAsync(ExportRequestCreateDTO request);
        Task<ExportRequestDTO> GetEarliestExportRequestAsync();
        Task<ExportRequestReturnDTO> UpdateExportRequestAsync(Guid importRequestId, ExportRequestStatusEnum status, string fileId, string result);
        Task<byte[]> ExportDataToExcel(List<CustomerDTO> customers);
    }
}
