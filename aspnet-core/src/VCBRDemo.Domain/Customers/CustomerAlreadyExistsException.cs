using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace VCBRDemo.Customers
{
    [Serializable]
    public class CustomerAlreadyExistsException : BusinessException
    {
        public CustomerAlreadyExistsException(string id)
            : base(VCBRDemoDomainErrorCodes.CustomerAlreadyExists)
        {
            WithData("Cusomer", id);
        }
    }
}
