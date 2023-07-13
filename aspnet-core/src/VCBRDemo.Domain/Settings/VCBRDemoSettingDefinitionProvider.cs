using Volo.Abp.Settings;

namespace VCBRDemo.Settings;

public class VCBRDemoSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(VCBRDemoSettings.MySetting1));
    }
}
