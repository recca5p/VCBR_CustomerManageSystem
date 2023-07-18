namespace VCBRDemo.Permissions;

public static class VCBRDemoPermissions
{
    public const string GroupName = "VCBRDemo";

    public static object AbpIdentity { get; set; }

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
    public static class Customers
    {
        public const string Default = GroupName + ".Customers";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string GetInfo = Default + ".GetInfo";
        public const string GetList = Default + ".GetList";
        public const string ImportFile = Default + ".ImportFile";
        public const string DetailInfo = Default + ".DetailInfo";
    }
}
