using Abp.AutoMapper;
using VoTanPhatVCBRDemo.Authentication.External;

namespace VoTanPhatVCBRDemo.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
