using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace VCBRDemo.Customers
{
    public class Customer : FullAuditedAggregateRoot<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public CustomerGenderEnum? Gender { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string IdentityNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public double? Balance { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }

        public Customer()
        {

        }

        internal Customer (Guid id,
            string firstName,
            string lastName,
            CustomerGenderEnum gender,
            string address,
            string email,
            [NotNull] string identityNumber,
            string phoneNumber,
            double balance,
            [NotNull]Guid userId,
            [NotNull]bool isActive)
        {
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Address = address;
            Email = email;
            IdentityNumber = identityNumber;
            PhoneNumber = phoneNumber;
            Balance = balance;
            UserId = userId;
            IsActive = isActive;
        }

        internal Customer ChangeFirstName([NotNull] string firstName)
        {
            SetFirstName(firstName);
            return this;
        }

        internal Customer ChangeLastName(string lastName)
        {
            SetLastName(lastName);
            return this;
        }

        internal Customer ChangeGender(CustomerGenderEnum gender)
        {
            SetGender(gender);
            return this;
        }

        internal Customer ChangeAddress(string address)
        {
            SetAddress(address);
            return this;
        }

        internal Customer ChangeEmail(string email)
        {
            SetEmail(email);
            return this;
        }

        internal Customer ChangePhoneNumber(string phoneNumber)
        {
            SetPhoneNumber(phoneNumber);
            return this;
        }

        internal Customer ChangeBalance(double balance)
        {
            SetBalance(balance);
            return this;
        }

        internal Customer ChangeIsActive(bool isActive)
        {
            SetIsActive(isActive);
            return this;
        }

        private void SetFirstName(string firstName)
        {
            FirstName = Check.NotNullOrWhiteSpace(firstName, nameof(firstName), maxLength: CustomerConsts.MaxLength);
        }

        private void SetLastName(string lastName)
        {
            LastName = Check.NotNullOrWhiteSpace(lastName, nameof(lastName), maxLength: CustomerConsts.MaxLength);
        }

        private void SetGender(CustomerGenderEnum gender)
        {
            Gender = gender;
        }

        private void SetAddress(string address)
        {
            Address = Check.NotNullOrWhiteSpace(address, nameof(address), maxLength: CustomerConsts.MaxLength);
        }

        private void SetEmail(string email)
        {
            Email = Check.NotNullOrWhiteSpace(email, nameof(email), maxLength: CustomerConsts.MaxLength);
        }

        private void SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), maxLength: 20);
        }

        private void SetBalance(double balance)
        {
            Balance = Check.Range(balance, nameof(balance), 0, 10000000000000);
        }

        private void SetIsActive(bool isActive)
        {
            IsActive = Check.NotNull(isActive, nameof(isActive));
        }
    }
}
