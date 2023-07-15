using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace VCBRDemo.Customers.DTOs
{
    public class CustomerCreateDTO
    {
        [StringLength(CustomerConsts.MaxLength)]
        public string? FirstName { get; set; }
        [StringLength(CustomerConsts.MaxLength)]
        public string? LastName { get; set; }
        public CustomerGenderEnum Gender { get; set; }
        [StringLength(CustomerConsts.MaxLength)]
        public string? Address { get; set; }
        [StringLength(CustomerConsts.MaxLength)]
        public string? Email { get; set; }
        [Required]
        [StringLength(CustomerConsts.MaxLength)]
        public string IdentityNumber { get; set; }
        [StringLength(CustomerConsts.MaxLength)]
        public string? PhoneNumber { get; set; }
        [Range(0, 1000000000000)]
        public double? Balance { get; set; }
        [DisableAuditing]
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        public string Password { get; set; }
    }
}
