using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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
        private readonly IConfiguration _configuration;
        public EfCoreCustomerRepository(IDbContextProvider<VCBRDemoDbContext> dbContextProvider,
            IConfiguration configuration) :  base(dbContextProvider) 
        {
            _configuration = configuration;
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
            string filter = null
            )
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    customer => customer.IdentityNumber.Contains(filter) || customer.Email.Contains(filter)
                    )
                .Where(_ => _.IsActive == true)
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


        public void AddCustomerToUserRole(Guid userId)
        {
            string connectionString = _configuration["ConnectionStrings:Default"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve role ID using a separate query
                string roleQuery = "SELECT Id FROM AbpRoles WHERE Name = 'user'";
                Guid roleId;

                using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
                {
                    roleId = (Guid)roleCommand.ExecuteScalar();
                }

                string insertQuery = $"INSERT INTO AbpUserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)";

                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@RoleId", roleId);
                    insertCommand.Parameters.AddWithValue("@UserId", userId);

                    insertCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
