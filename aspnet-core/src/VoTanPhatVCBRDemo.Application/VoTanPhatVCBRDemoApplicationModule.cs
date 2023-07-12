using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using VoTanPhatVCBRDemo.Authorization;

namespace VoTanPhatVCBRDemo
{
    [DependsOn(
        typeof(VoTanPhatVCBRDemoCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class VoTanPhatVCBRDemoApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<VoTanPhatVCBRDemoAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(VoTanPhatVCBRDemoApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
