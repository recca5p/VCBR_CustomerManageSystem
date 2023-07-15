using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VCBRDemo.Customers.DTOs
{
    public class CustomerUpdateDTO
    {
        [StringLength(CustomerConsts.MaxLength)]
        public string? FirstName { get; set; }
        [StringLength(CustomerConsts.MaxLength)]
        public string? LastName { get; set; }
        public CustomerGenderEnum? Gender { get; set; }
        [StringLength(CustomerConsts.MaxLength)]
        public string? Address { get; set; }
        [StringLength(CustomerConsts.MaxLength)]
        public string? Email { get; set; }
        [StringLength(CustomerConsts.MaxLength)]
        public string? PhoneNumber { get; set; }
        [Required]
        [Range(0, 1000000000000)]
        public double? Balance { get; set; }
        public bool? IsActive { get; set; }
    }
}
