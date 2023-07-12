using Abp.Application.Services.Dto;

namespace VoTanPhatVCBRDemo.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

