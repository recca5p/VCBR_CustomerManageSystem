using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace VoTanPhatVCBRDemo.Localization
{
    public static class VoTanPhatVCBRDemoLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(VoTanPhatVCBRDemoConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(VoTanPhatVCBRDemoLocalizationConfigurer).GetAssembly(),
                        "VoTanPhatVCBRDemo.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
