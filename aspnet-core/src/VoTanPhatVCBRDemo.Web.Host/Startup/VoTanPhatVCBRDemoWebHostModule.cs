using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using VoTanPhatVCBRDemo.Configuration;

namespace VoTanPhatVCBRDemo.Web.Host.Startup
{
    [DependsOn(
       typeof(VoTanPhatVCBRDemoWebCoreModule))]
    public class VoTanPhatVCBRDemoWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public VoTanPhatVCBRDemoWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VoTanPhatVCBRDemoWebHostModule).GetAssembly());
        }
    }
}
