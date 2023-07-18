using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers;
using VCBRDemo.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace VCBRDemo.ExportRequests
{
    public class EfCoreExportRequestRepository : EfCoreRepository<VCBRDemoDbContext, ExportRequest, Guid>, IExportRequestRepository
    {
        public EfCoreExportRequestRepository(IDbContextProvider<VCBRDemoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<ExportRequest> GetEarliestExportRequestAsync()
        {
            var dbset = await GetDbSetAsync();

            return await dbset
                .Where(_ => _.RequestStatus == ExportRequestStatusEnum.Created)
                .OrderBy(_ => _.CreationTime)
                .FirstOrDefaultAsync();
        }
    }
}
