using System.Collections.Generic;

namespace VoTanPhatVCBRDemo.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
