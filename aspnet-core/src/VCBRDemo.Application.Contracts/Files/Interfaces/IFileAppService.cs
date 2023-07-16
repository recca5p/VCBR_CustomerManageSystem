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

        Task<UploadFileResponseDTO> UploadFileAsync(IFormFile file, string key);

        //Task<bool> DeleteFileAsync(string fileName, string versionId = "");
    }
}
