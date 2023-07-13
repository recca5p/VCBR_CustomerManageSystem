using VCBRDemo.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace VCBRDemo;

[DependsOn(
    typeof(VCBRDemoEntityFrameworkCoreTestModule)
    )]
public class VCBRDemoDomainTestModule : AbpModule
{

}
