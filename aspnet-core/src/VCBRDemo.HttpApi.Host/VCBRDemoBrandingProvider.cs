using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace VCBRDemo;

[Dependency(ReplaceServices = true)]
public class VCBRDemoBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "VCBRDemo";
}
