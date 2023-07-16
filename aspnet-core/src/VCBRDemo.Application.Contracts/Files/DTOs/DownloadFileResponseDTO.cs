using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VCBRDemo.Files.DTOs
{
    public class DownloadFileResponseDTO : UploadFileResponseDTO
    {
        public Stream FileStream { get; set; }
        public string ContentType { get; set; }
    }
}
