using System;
using System.Collections.Generic;
using System.Text;
using VCBRDemo.Localization;
using Volo.Abp.Application.Services;

namespace VCBRDemo;

/* Inherit your application services from this class.
 */
public abstract class VCBRDemoAppService : ApplicationService
{
    protected VCBRDemoAppService()
    {
        LocalizationResource = typeof(VCBRDemoResource);
    }
}
