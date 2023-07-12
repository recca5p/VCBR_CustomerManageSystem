using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using VoTanPhatVCBRDemo.Configuration.Dto;

namespace VoTanPhatVCBRDemo.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : VoTanPhatVCBRDemoAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
