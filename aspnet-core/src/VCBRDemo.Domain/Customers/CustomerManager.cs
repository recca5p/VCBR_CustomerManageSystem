using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace VCBRDemo.Customers
{
    public class CustomerManager : DomainService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerManager(ICustomerRepository customerRepository) 
        { 
            _customerRepository = customerRepository;
        }

        public async Task<Customer> CreateAsync (
                string firstName,
                string lastName,
                CustomerGenderEnum gender,
                string address,
                string email,
                [NotNull] string identityNumber,
                string phoneNumber,
                double balance,
                [NotNull]Guid userId,
                [NotNull]bool isActive
            )
        {
            Check.NotNullOrWhiteSpace(identityNumber, nameof(identityNumber));
            Check.NotNull(userId, nameof(userId));

            Customer existingCustomer = await _customerRepository.FindByIdentityNumberAsync(identityNumber);

            if (existingCustomer != null) 
            { 
                throw new CustomerAlreadyExistsException(identityNumber);
            }

            return new Customer(
                    GuidGenerator.Create(),
                    firstName,
                    lastName,
                    gender,
                    address,
                    email,
                    identityNumber,
                    phoneNumber,
                    balance,
                    userId,
                    isActive
                );
        }

        public async Task ChangeFirstNameAsync(
                [NotNull] Customer customer,
                [NotNull] string newFirstName)
        {
            Check.NotNull(customer, nameof(customer));
            Check.NotNullOrWhiteSpace(newFirstName, nameof(newFirstName));

            customer.ChangeFirstName(newFirstName);
        }

        public async Task ChangeLastNameAsync(
            [NotNull] Customer customer,
            string newLastName)
        {
            Check.NotNull(customer, nameof(customer));
            Check.NotNullOrWhiteSpace(newLastName, nameof(newLastName));

            customer.ChangeLastName(newLastName);
        }

        public async Task ChangeGenderAsync(
            [NotNull] Customer customer,
            CustomerGenderEnum newGender)
        {
            Check.NotNull(customer, nameof(customer));
            Check.NotNull(newGender, nameof(newGender));

            customer.ChangeGender(newGender);
        }

        public async Task ChangeAddressAsync(
            [NotNull] Customer customer,
            string newAddress)
        {
            Check.NotNull(customer, nameof(customer));
            Check.NotNullOrWhiteSpace(newAddress, nameof(newAddress));

            customer.ChangeAddress(newAddress);
        }

        public async Task ChangeEmailAsync(
            [NotNull] Customer customer,
            string newEmail)
        {
            Check.NotNull(customer, nameof(customer));
            Check.NotNullOrWhiteSpace(newEmail, nameof(newEmail));

            customer.ChangeEmail(newEmail);
        }

        public async Task ChangePhoneNumberAsync(
            [NotNull] Customer customer,
            string newPhoneNumber)
        {
            Check.NotNull(customer, nameof(customer));
            Check.NotNullOrWhiteSpace(newPhoneNumber, nameof(newPhoneNumber));

            customer.ChangePhoneNumber(newPhoneNumber);
        }

        public async Task ChangeBalanceAsync(
            [NotNull] Customer customer,
            double newBalance)
        {
            Check.NotNull(customer, nameof(customer));
            Check.Range(newBalance, nameof(newBalance), 0, 1000000000000);

            customer.ChangeBalance(newBalance);
        }

        public async Task ChangeIsActiveAsync(
            [NotNull] Customer customer,
            bool isActive)
        {
            Check.NotNull(customer, nameof(customer));
            Check.NotNull(isActive, nameof(isActive));
            customer.ChangeIsActive(isActive);
        }
    }
}
