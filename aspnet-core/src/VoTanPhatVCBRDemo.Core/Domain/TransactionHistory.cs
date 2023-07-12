using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoTanPhatVCBRDemo.Domain
{
    public class TransactionHistory : AuditedAggregateRoot<int>
    {
        public double Amount { get; set; }
        public bool IsDeleted { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
