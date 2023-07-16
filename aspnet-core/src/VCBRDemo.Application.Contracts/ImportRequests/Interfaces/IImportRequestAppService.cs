using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.ImportRequests.DTOs;
using Volo.Abp.Application.Services;

namespace VCBRDemo.ImportRequests.Interfaces
{
    public interface IImportRequestAppService : IApplicationService
    {
        Task<ImportRequestDTO> ImportCustomersByFileAsync(ImportRequestCreateDTO file);
    }
}
