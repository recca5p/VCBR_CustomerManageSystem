using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.BackgroundWorkers.Quartz;
using Quartz;
using Microsoft.Extensions.DependencyInjection;
using VCBRDemo.Jobs;
using Volo.Abp.Quartz;

namespace VCBRDemo;

[DependsOn(
    typeof(VCBRDemoDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(VCBRDemoApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
    public class VCBRDemoApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<VCBRDemoApplicationModule>();
        });

        context.Services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionScopedJobFactory();

            // Register the job, loading the schedule from configuration
            q.AddJobAndTrigger<ImportDataJob>(context.Services.GetConfiguration());
        });

        context.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}
