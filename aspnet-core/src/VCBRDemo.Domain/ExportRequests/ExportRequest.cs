using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace VCBRDemo.ExportRequests
{
    public class ExportRequest : AuditedEntity<Guid>
    {
        public string? Filter { get; set; }
        public string? FileId { get; set; }
        public ExportRequestStatusEnum RequestStatus { get; set; }
        public string? Result { get; set; }
    }
}
