using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers;
using VCBRDemo.Customers.DTOs;
using VCBRDemo.Customers.Interfaces;
using VCBRDemo.Permissions;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace VCBRDemo.Customers
{
    public class CustomerAppService : VCBRDemoAppService, ICustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerManager _customerManager;
        private readonly IIdentityUserAppService _identityUserService;
        private readonly IAccountAppService _accountAppService;

        public CustomerAppService(
                ICustomerRepository customerRepository,
                CustomerManager customerManager,
                IIdentityUserAppService identityUserService,
                IAccountAppService accountAppService
            )
        {
            _customerManager = customerManager;
            _customerRepository = customerRepository;
            _identityUserService = identityUserService;
            _accountAppService = accountAppService;
        }

        [Authorize(VCBRDemoPermissions.Customers.GetInfo)]
        public async Task<CustomerDTO> GetAsync(Guid id)
        {
            try
            {
                Customer customer = await _customerRepository.FindByUserIdAsync(id);
                if (customer == null)
                    throw new UserFriendlyException("Data not found");
                return ObjectMapper.Map<Customer, CustomerDTO>(customer);
            }
            catch( Exception ex )
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<CustomerDTO> FindByIdentityNumberAsync(string identityNumber)
        {
            Customer customer = await _customerRepository.FindByIdentityNumberAsync(identityNumber);

            return ObjectMapper.Map<Customer, CustomerDTO>(customer);
        }


        [Authorize(VCBRDemoPermissions.Customers.GetList)]
        public async Task<PagedResultDto<CustomerDTO>> GetListAsync(CustomerFilterListDTO input)
        {
            try
            {
                if (input.Sorting.IsNullOrWhiteSpace())
                {
                    input.Sorting = nameof(Customer.FirstName);
                }

                List<Customer> customers = await _customerRepository.GetListAsync(
                        input.SkipCount,
                        input.MaxResultCount,
                        input.Sorting,
                        input.FromDate,
                        input.ToDate,
                        input.Filter
                    );
                if (customers.IsNullOrEmpty())
                    throw new UserFriendlyException("Data not found");

                int totalCount = input.Filter == null
                    ? await _customerRepository.CountAsync(c => c.IsActive == true && (c.CreationTime >= input.FromDate && c.CreationTime <= input.ToDate))
                    : await _customerRepository.CountAsync(customer => customer.FirstName.Contains(input.Filter) && (customer.CreationTime >= input.FromDate && customer.CreationTime <= input.ToDate)); 
                
                List<CustomerDTO> result = ObjectMapper.Map<List<Customer>, List<CustomerDTO>>(customers);

                return new PagedResultDto<CustomerDTO>(
                        totalCount,
                        result);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [Authorize(VCBRDemoPermissions.Customers.Create)]
        public async Task<CustomerDTO> CreateAsync(CustomerCreateDTO input)
        {
            try
            {
                /*Register AbpUser to generate an account for customer*/
                RegisterDto accRes = new RegisterDto
                {
                    EmailAddress = input.Email,
                    UserName = input.IdentityNumber,
                    Password = input.Password,
                    AppName = "VCBRDemo"
                };

                IdentityUserDto account = await _accountAppService.RegisterAsync(accRes);

                /*Assign user role*/
                IdentityUserUpdateRolesDto roles = new IdentityUserUpdateRolesDto
                {
                    RoleNames = new string[] {"user"}
                };
                /*Always assign "user" role*/
                await _identityUserService.UpdateRolesAsync(account.Id, roles);

                /*Create customer info*/
                Customer customer = await _customerManager.CreateAsync(
                    input.FirstName,
                    input.LastName,
                    (CustomerGenderEnum)input.Gender,
                    input.Address,
                    input.Email,
                    input.IdentityNumber,
                    input.PhoneNumber,
                    (double)input.Balance,
                    account.Id,
                    true
                );

                await _customerRepository.InsertAsync(customer);

                return ObjectMapper.Map<Customer, CustomerDTO>(customer);
            }
            catch (Exception ex)
            {
                if (ex is Volo.Abp.Validation.AbpValidationException)
                {
                    var voloEx = new Volo.Abp.Validation.AbpValidationException();
                    voloEx = (Volo.Abp.Validation.AbpValidationException)ex;
                    var message = voloEx.ValidationErrors
                        .Select(err => err.ToString())
                        .Aggregate(string.Empty, (current, next) => string.Format("{0}\n{1}", current, next));
                    throw new UserFriendlyException(message);

                }
                throw new UserFriendlyException(ex.Message);
            }
        }

        [Authorize(VCBRDemoPermissions.Customers.Edit)]
        public async Task UpdateAsync(string identityNumber, CustomerUpdateDTO input)
        {
            try
            {
                Customer customer = await _customerRepository.FindByIdentityNumberAsync(identityNumber);
                if (customer == null)
                    throw new UserFriendlyException("Identity number is not found");

                if (customer.FirstName != input.FirstName && !input.FirstName.IsNullOrEmpty())
                {
                    await _customerManager.ChangeFirstNameAsync(customer, input.FirstName);
                }

                if (customer.LastName != input.LastName && !input.LastName.IsNullOrEmpty())
                {
                    await _customerManager.ChangeLastNameAsync(customer, input.LastName);
                }

                if (customer.Gender != input.Gender && input.Gender != null)
                {
                    await _customerManager.ChangeGenderAsync(customer, (CustomerGenderEnum)input.Gender);
                }

                if (customer.Address != input.Address && !input.Address.IsNullOrEmpty())
                {
                    await _customerManager.ChangeAddressAsync(customer, input.Address);
                }

                if (customer.Email != input.Email && !input.Email.IsNullOrEmpty())
                {
                    await _customerManager.ChangeEmailAsync(customer, input.Email);
                }

                if (customer.PhoneNumber != input.PhoneNumber && !input.PhoneNumber.IsNullOrEmpty())
                {
                    await _customerManager.ChangePhoneNumberAsync(customer, input.PhoneNumber);
                }

                if (customer.Balance != (double)input.Balance)
                {
                    await _customerManager.ChangeBalanceAsync(customer, (double)input.Balance);
                }

                if (customer.IsActive != input.IsActive && input.IsActive != null)
                {
                    await _customerManager.ChangeIsActiveAsync(customer, (bool)input.IsActive);
                }

                await _customerRepository.UpdateAsync(customer);
                /*Update AbpUser*/
            }
            catch (Exception ex)
            {
                if (ex is Volo.Abp.Validation.AbpValidationException)
                {
                    var voloEx = new Volo.Abp.Validation.AbpValidationException();
                    voloEx = (Volo.Abp.Validation.AbpValidationException)ex;
                    var message = voloEx.ValidationErrors
                        .Select(err => err.ToString())
                        .Aggregate(string.Empty, (current, next) => string.Format("{0}\n{1}", current, next));
                    throw new UserFriendlyException(message);

                }
                throw new UserFriendlyException(ex.Message);
            }
        }

        [Authorize(VCBRDemoPermissions.Customers.Delete)]
        public async Task DeleteCustomerAsync(string identityNumber)
        {
            try
            {
                Customer customer = await _customerRepository.FindByIdentityNumberAsync(identityNumber);
                if (customer == null)
                    throw new UserFriendlyException("The identity number is not exist");

                if (customer.IsActive == false)
                    throw new UserFriendlyException("The customer is disabled");

                /*Inactive customer*/
                customer.IsActive = false;

                await _customerRepository.UpdateAsync(customer);
                /*Inactive user account*/
                IdentityUserDto userAcc = await _identityUserService.GetAsync(customer.UserId);

                IdentityUserUpdateDto updateAcc = ObjectMapper.Map<IdentityUserDto, IdentityUserUpdateDto>(userAcc);

                updateAcc.IsActive = false;

                await _identityUserService.UpdateAsync(customer.UserId, updateAcc);
            }
            catch (Exception ex)
            {
                if (ex is Volo.Abp.Validation.AbpValidationException)
                {
                    var voloEx = new Volo.Abp.Validation.AbpValidationException();
                    voloEx = (Volo.Abp.Validation.AbpValidationException)ex;
                    var message = voloEx.ValidationErrors
                        .Select(err => err.ToString())
                        .Aggregate(string.Empty, (current, next) => string.Format("{0}\n{1}", current, next));
                    throw new UserFriendlyException(message);

                }
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
