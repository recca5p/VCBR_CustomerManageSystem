using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace VCBRDemo.ExportRequests.DTOs
{
    public class ExportRequestCrudDTO : AuditedEntityDto<Guid>
    {
        public string? Filter { get; set; }
        public string? FileId { get; set; }
        public ExportRequestStatusEnum RequestStatus { get; set; }
        public string? Result { get; set; }
    }
}
