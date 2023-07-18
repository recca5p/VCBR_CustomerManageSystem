using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers.DTOs;
using VCBRDemo.Files.DTOs;
using Volo.Abp.Application.Services;

namespace VCBRDemo.Files.Interfaces
{
    public interface IFileAppService : IApplicationService
    {
        Task<DownloadFileResponseDTO> DownloadFileAsync(string key);

        Task<UploadFileResponseDTO> UploadFileAsync(byte[] file, string key, string contentType);

        Task<DeleteFileResponseDTO> DeleteFileAsync(string key);
    }
}
