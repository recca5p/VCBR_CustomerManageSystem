using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoTanPhatVCBRDemo.Domain
{
    public class TransactionHistory : AuditedAggregateRoot<Guid>
    {
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
