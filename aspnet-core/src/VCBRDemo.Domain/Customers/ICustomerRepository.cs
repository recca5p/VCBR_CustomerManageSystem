using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace VCBRDemo.Customers
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<Customer> FindByIdentityNumberAsync(string identityNumber);
        Task<List<Customer>> GetListAsync(int skipCount, int maxResultCount, string sorting, string? filter);
    }
}
