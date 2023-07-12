using System.Threading.Tasks;
using Abp.Application.Services;
using VoTanPhatVCBRDemo.Authorization.Accounts.Dto;

namespace VoTanPhatVCBRDemo.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
