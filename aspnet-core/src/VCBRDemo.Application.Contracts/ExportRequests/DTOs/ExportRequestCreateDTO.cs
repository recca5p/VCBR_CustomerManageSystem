using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using VCBRDemo.Customers.DTOs;
using VCBRDemo.Files.DTOs;

namespace VCBRDemo.ExportRequests.DTOs
{
    public class ExportRequestCreateDTO
    {
        public string Filter { get; set; }
    }
}
