using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoTanPhatVCBRDemo.Domain
{
    public class Customer : AuditedEntity<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<TransactionHistory> TransactionHistories { get; set; }
    }
}
