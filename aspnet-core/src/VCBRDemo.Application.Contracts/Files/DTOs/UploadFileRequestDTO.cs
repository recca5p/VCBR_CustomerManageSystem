using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VCBRDemo.Files.DTOs
{
    public class UploadFileRequestDTO
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}
