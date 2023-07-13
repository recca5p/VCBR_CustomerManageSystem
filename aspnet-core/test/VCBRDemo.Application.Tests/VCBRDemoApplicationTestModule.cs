using Volo.Abp.Modularity;

namespace VCBRDemo;

[DependsOn(
    typeof(VCBRDemoApplicationModule),
    typeof(VCBRDemoDomainTestModule)
    )]
public class VCBRDemoApplicationTestModule : AbpModule
{

}
