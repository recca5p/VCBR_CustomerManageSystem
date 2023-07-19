using System;
using System.Collections.Generic;
using System.Text;
using VCBRDemo.Files;
using Volo.Abp.Application.Dtos;

namespace VCBRDemo.ImportRequests.DTOs
{
    public class ImportCRUDDTO : AuditedEntityDto<Guid>
    {
        public string Filename { get; set; }
        public string Extension { get; set; }
        public string FileId { get; set; }
        public string RequestStatus { get; set; }
        public string Result { get; set; }
        public string ReportId { get; set; }
    }
}
