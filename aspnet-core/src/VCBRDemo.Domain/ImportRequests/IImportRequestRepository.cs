using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace VCBRDemo.ImportRequests
{
    public interface IImportRequestRepository : IRepository<ImportRequest, Guid>
    {
        Task<ImportRequest> GetEarliestImportrequestAsync();
    }
}
