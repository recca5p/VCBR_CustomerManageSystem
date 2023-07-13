using VCBRDemo.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace VCBRDemo.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(VCBRDemoEntityFrameworkCoreModule),
    typeof(VCBRDemoApplicationContractsModule)
    )]
public class VCBRDemoDbMigratorModule : AbpModule
{
}
