using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Files;
using VCBRDemo.ImportRequests.DTOs;
using Volo.Abp.Application.Services;

namespace VCBRDemo.ImportRequests.Interfaces
{
    public interface IImportRequestAppService : IApplicationService
    {
        Task<ImportRequestResponseDTO> ImportCustomersByFileAsync(ImportRequestCreateDTO file);
        Task<ImportDataIntoDatabaseDTO> ImportDataIntoDatabaseAsync(byte[] fileArray);
        Task<ImportRequestDTO> GetEarliestImportrequestAsync();
        Task<ImportRequestResponseDTO> UpdateImportRequestStatusAsync(Guid importRequestId, ImportRequestStatusEnum status, string reportId);
    }
}
