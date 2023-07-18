using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers.DTOs;
using VCBRDemo.Files.DTOs;
using VCBRDemo.Files.Interfaces;
using Volo.Abp;

namespace VCBRDemo.Customers
{
    [RemoteService(IsEnabled = false)]
    public class FileAppService : IFileAppService
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _awsS3Client;
        private readonly IConfiguration _configuration;


        public FileAppService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<UploadFileResponseDTO> UploadFileAsync(byte[] file, string key, string contentType)
        {
            try
            {
                Stream stream = new MemoryStream(file);

                var accesskey = _configuration["AWS:AwsAccessKey"];
                var secretkey = _configuration["AWS:AwsSecretAccessKey"];
                RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast1;
                var bucketName = _configuration["AWS:BucketName"];
                var s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);
                var fileTransferUtility = new TransferUtility(s3Client);//create an object for TransferUtility

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = key,
                    BucketName = bucketName,
                    ContentType = contentType
                };

                await fileTransferUtility.UploadAsync(uploadRequest);
                return new UploadFileResponseDTO { Success = true};
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DownloadFileResponseDTO> DownloadFileAsync(string key)
        {
            try
            {
                var accesskey = _configuration["AWS:AwsAccessKey"];
                var secretkey = _configuration["AWS:AwsSecretAccessKey"];
                RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast1;
                var bucketName = _configuration["AWS:BucketName"];
                var downloadRequest = new Amazon.S3.Model.GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };
                var s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);

                var response = await s3Client.GetObjectAsync(downloadRequest);
                var memoryStream = new MemoryStream();

                await response.ResponseStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                return new DownloadFileResponseDTO
                {
                    Success = true,
                    FileStream = memoryStream,
                    ContentType = response.Headers.ContentType
                };
            }
            catch (Exception ex)
            {
                return new DownloadFileResponseDTO { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<DeleteFileResponseDTO> DeleteFileAsync(string key)
        {
            try
            {
                var accesskey = _configuration["AWS:AwsAccessKey"];
                var secretkey = _configuration["AWS:AwsSecretAccessKey"];
                RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast1;
                var bucketName = _configuration["AWS:BucketName"];

                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                var s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);

                var response = await s3Client.DeleteObjectAsync(deleteRequest);

                return new DeleteFileResponseDTO { Success = response.HttpStatusCode == HttpStatusCode.NoContent };
            }
            catch (Exception ex)
            {
                return new DeleteFileResponseDTO { Success = false, ErrorMessage = ex.Message };
            }
        }

    }
}
