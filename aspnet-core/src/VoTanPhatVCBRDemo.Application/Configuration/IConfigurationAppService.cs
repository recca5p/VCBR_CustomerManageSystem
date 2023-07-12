using System.Threading.Tasks;
using VoTanPhatVCBRDemo.Configuration.Dto;

namespace VoTanPhatVCBRDemo.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
