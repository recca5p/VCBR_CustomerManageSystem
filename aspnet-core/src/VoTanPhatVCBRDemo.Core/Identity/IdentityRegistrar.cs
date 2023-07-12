using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VoTanPhatVCBRDemo.Authorization;
using VoTanPhatVCBRDemo.Authorization.Roles;
using VoTanPhatVCBRDemo.Authorization.Users;
using VoTanPhatVCBRDemo.Editions;
using VoTanPhatVCBRDemo.MultiTenancy;

namespace VoTanPhatVCBRDemo.Identity
{
    public static class IdentityRegistrar
    {
        public static IdentityBuilder Register(IServiceCollection services)
        {
            services.AddLogging();

            return services.AddAbpIdentity<Tenant, User, Role>()
                .AddAbpTenantManager<TenantManager>()
                .AddAbpUserManager<UserManager>()
                .AddAbpRoleManager<RoleManager>()
                .AddAbpEditionManager<EditionManager>()
                .AddAbpUserStore<UserStore>()
                .AddAbpRoleStore<RoleStore>()
                .AddAbpLogInManager<LogInManager>()
                .AddAbpSignInManager<SignInManager>()
                .AddAbpSecurityStampValidator<SecurityStampValidator>()
                .AddAbpUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddPermissionChecker<PermissionChecker>()
                .AddDefaultTokenProviders();
        }
    }
}
