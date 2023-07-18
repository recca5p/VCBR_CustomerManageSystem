using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers.DTOs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace VCBRDemo.Customers.Interfaces
{
    public interface ICustomerAppService : IApplicationService
    {
        Task<CustomerDTO> GetAsync (Guid id);
        Task<PagedResultDto<CustomerDTO>> GetListAsync(CustomerFilterListDTO input);
        Task<CustomerDTO> CreateAsync (CustomerCreateDTO input);
        Task UpdateAsync(string identityNumber, CustomerUpdateDTO input);
        Task DeleteCustomerAsync (string identityNumber);
        Task<CustomerDTO> FindByIdentityNumberAsync(string identityNumber);
        Task<CustomerDTO> CreateUsingByWorkerAsync(CustomerCreateDTO input);
        void AddCustomerToUserRole(List<Guid> userIds);
    }
}
