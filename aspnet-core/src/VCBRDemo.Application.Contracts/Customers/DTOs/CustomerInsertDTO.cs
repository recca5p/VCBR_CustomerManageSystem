using System;
using System.Collections.Generic;
using System.Text;
using VCBRDemo.Files;

namespace VCBRDemo.Customers.DTOs
{
    public class CustomerInsertDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public CustomerGenderEnum? Gender { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string IdentityNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public double? Balance { get; set; }
        public ImportRequestStatusEnum ImportStatus { get; set; }
        public string Message { get; set; }
    }
}
