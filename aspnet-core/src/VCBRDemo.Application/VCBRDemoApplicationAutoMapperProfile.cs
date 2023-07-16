using AutoMapper;
using VCBRDemo.Customers;
using VCBRDemo.Customers.DTOs;
using VCBRDemo.ImportRequests;
using VCBRDemo.ImportRequests.DTOs;
using Volo.Abp.Identity;

namespace VCBRDemo;

public class VCBRDemoApplicationAutoMapperProfile : Profile
{
    public VCBRDemoApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Customer, CustomerDTO>()
            .ForMember(src => src.Gender, opt => opt.MapFrom(_ => _.Gender.ToString()))
            .ForMember(src => src.CreatedTime, opt => opt.MapFrom(_ => _.CreationTime.ToString("dd/MM/yyyy HH:mm")));
        CreateMap<CustomerCreateDTO, Customer>();
        CreateMap<IdentityUserDto, IdentityUserUpdateDto>();
        CreateMap<ImportRequest, ImportRequestDTO>();
    }
}
