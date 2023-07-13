using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoTanPhatVCBRDemo.Domain
{
    public class Customer : AuditedEntity<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public double Balance { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<TransactionHistory> TransactionHistories { get; set; }
        public long UserId { get; set; }
    }
}
