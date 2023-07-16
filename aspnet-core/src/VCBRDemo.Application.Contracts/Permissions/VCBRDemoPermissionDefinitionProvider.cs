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
        var customersPermission = myGroup.AddPermission(
            VCBRDemoPermissions.Customers.Default, L("Permission:Customers"));
        customersPermission.AddChild(
            VCBRDemoPermissions.Customers.Create, L("Permission:Customers.Create"));
        customersPermission.AddChild(
            VCBRDemoPermissions.Customers.Edit, L("Permission:Customers.Edit"));
        customersPermission.AddChild(
            VCBRDemoPermissions.Customers.Delete, L("Permission:Customers.Delete"));
        customersPermission.AddChild(
            VCBRDemoPermissions.Customers.GetInfo, L("Permission:Customers.GetInfo"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VCBRDemoResource>(name);
    }
}
