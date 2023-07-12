using Abp.MultiTenancy;
using VoTanPhatVCBRDemo.Authorization.Users;

namespace VoTanPhatVCBRDemo.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
