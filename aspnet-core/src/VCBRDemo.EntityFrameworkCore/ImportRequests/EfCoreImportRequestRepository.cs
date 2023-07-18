using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers;
using VCBRDemo.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace VCBRDemo.ImportRequests
{
    public class EfCoreImportRequestRepository : EfCoreRepository<VCBRDemoDbContext, ImportRequest, Guid>, IImportRequestRepository
    {
        private readonly IConfiguration _configuration;
        public EfCoreImportRequestRepository(IDbContextProvider<VCBRDemoDbContext> dbContextProvider, IConfiguration configuration) : base(dbContextProvider)
        {
            _configuration = configuration;
        }

        public async Task<ImportRequest> GetEarliestImportrequestAsync()
        {
            var dbset = await GetDbSetAsync();

            return await dbset
                .Where(_ => _.RequestStatus == Files.ImportRequestStatusEnum.Created)
                .OrderBy(_ => _.CreationTime)
                .FirstOrDefaultAsync();
        }
    }
}
