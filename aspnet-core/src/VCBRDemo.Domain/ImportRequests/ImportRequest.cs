using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Files;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace VCBRDemo.ImportRequests
{
    public class ImportRequest : AuditedEntity<Guid>
    {
        public string Filename { get; set; }
        public string Extension { get; set; }
        public string FileId { get; set; }
        public ImportRequestStatusEnum RequestStatus { get; set; }
        public string Result { get; set; }
        public string ReportId { get; set; }
    }
}
