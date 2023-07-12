using Abp.Application.Services;
using VoTanPhatVCBRDemo.MultiTenancy.Dto;

namespace VoTanPhatVCBRDemo.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

