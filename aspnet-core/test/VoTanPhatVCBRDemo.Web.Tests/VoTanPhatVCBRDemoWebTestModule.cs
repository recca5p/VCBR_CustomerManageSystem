using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using VoTanPhatVCBRDemo.EntityFrameworkCore;
using VoTanPhatVCBRDemo.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace VoTanPhatVCBRDemo.Web.Tests
{
    [DependsOn(
        typeof(VoTanPhatVCBRDemoWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class VoTanPhatVCBRDemoWebTestModule : AbpModule
    {
        public VoTanPhatVCBRDemoWebTestModule(VoTanPhatVCBRDemoEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VoTanPhatVCBRDemoWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(VoTanPhatVCBRDemoWebMvcModule).Assembly);
        }
    }
}