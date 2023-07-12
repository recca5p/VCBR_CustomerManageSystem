using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using VoTanPhatVCBRDemo.Configuration;
using VoTanPhatVCBRDemo.EntityFrameworkCore;
using VoTanPhatVCBRDemo.Migrator.DependencyInjection;

namespace VoTanPhatVCBRDemo.Migrator
{
    [DependsOn(typeof(VoTanPhatVCBRDemoEntityFrameworkModule))]
    public class VoTanPhatVCBRDemoMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public VoTanPhatVCBRDemoMigratorModule(VoTanPhatVCBRDemoEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(VoTanPhatVCBRDemoMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                VoTanPhatVCBRDemoConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VoTanPhatVCBRDemoMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
