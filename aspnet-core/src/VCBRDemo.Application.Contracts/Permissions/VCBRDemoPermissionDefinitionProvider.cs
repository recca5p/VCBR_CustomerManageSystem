using VCBRDemo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace VCBRDemo.Permissions;

public class VCBRDemoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(VCBRDemoPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(VCBRDemoPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VCBRDemoResource>(name);
    }
}
