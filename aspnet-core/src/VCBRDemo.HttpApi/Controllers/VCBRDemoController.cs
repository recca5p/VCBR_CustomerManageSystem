using VCBRDemo.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace VCBRDemo.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class VCBRDemoController : AbpControllerBase
{
    protected VCBRDemoController()
    {
        LocalizationResource = typeof(VCBRDemoResource);
    }
}
