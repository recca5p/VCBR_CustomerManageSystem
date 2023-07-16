﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace VCBRDemo.Customers
{
    public class EfCoreCustomerRepository : EfCoreRepository<VCBRDemoDbContext, Customer, Guid>, ICustomerRepository
    {
        public EfCoreCustomerRepository(IDbContextProvider<VCBRDemoDbContext> dbContextProvider) :  base(dbContextProvider) 
        {

        }

        public async Task<Customer> FindByIdentityNumberAsync(string identityNumber)
        {
            var dbSet = await GetDbSetAsync();

            return await dbSet.FirstOrDefaultAsync(customer => customer.IdentityNumber == identityNumber);
        }

        public async Task<List<Customer>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting, 
            DateTime fromDate,
            DateTime toDate,
            string filter = null
            )
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    customer => customer.IdentityNumber.Contains(filter)
                    )
                .Where(c => c.IsActive == true && (c.CreationTime >= fromDate && c.CreationTime <= toDate))
                .OrderBy(_ => sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<Customer> FindByUserIdAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(c => c.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
