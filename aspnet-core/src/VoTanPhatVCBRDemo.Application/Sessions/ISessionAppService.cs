using System.Threading.Tasks;
using Abp.Application.Services;
using VoTanPhatVCBRDemo.Sessions.Dto;

namespace VoTanPhatVCBRDemo.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
