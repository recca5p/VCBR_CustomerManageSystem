using Abp.Authorization;
using VoTanPhatVCBRDemo.Authorization.Roles;
using VoTanPhatVCBRDemo.Authorization.Users;

namespace VoTanPhatVCBRDemo.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
