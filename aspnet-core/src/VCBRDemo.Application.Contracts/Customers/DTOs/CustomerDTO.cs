using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace VCBRDemo.Customers.DTOs
{
    public class CustomerDTO : EntityDto<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string IdentityNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public double? Balance { get; set; }
        public string? CreatedTime { get; set; }
        public bool? IsActive { get; set; }
    }
}
