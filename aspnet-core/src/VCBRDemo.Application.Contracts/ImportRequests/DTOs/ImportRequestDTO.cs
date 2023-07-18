using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace VCBRDemo.ImportRequests.DTOs
{
    public class ImportRequestDTO : AuditedEntityDto<Guid>
    {
        public string FileId { get; set; }
    }
}
