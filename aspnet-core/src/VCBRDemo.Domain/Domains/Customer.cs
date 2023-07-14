using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace VCBRDemo.Domains
{
    public class Customer : AuditedEntity<Guid>
    {
    }
}
