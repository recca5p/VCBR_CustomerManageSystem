using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace VoTanPhatVCBRDemo.Controllers
{
    public abstract class VoTanPhatVCBRDemoControllerBase: AbpController
    {
        protected VoTanPhatVCBRDemoControllerBase()
        {
            LocalizationSourceName = VoTanPhatVCBRDemoConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
