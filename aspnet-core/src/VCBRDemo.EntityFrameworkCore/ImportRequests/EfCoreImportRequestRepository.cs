using System;
using System.Collections.Generic;
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
        public EfCoreImportRequestRepository(IDbContextProvider<VCBRDemoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
