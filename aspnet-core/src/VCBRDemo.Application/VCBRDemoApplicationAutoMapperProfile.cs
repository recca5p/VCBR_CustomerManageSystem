using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using VCBRDemo.Customers;
using VCBRDemo.Customers.DTOs;
using VCBRDemo.ExportRequests;
using VCBRDemo.ExportRequests.DTOs;
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
        CreateMap<ExportRequest, ExportRequestDTO>();
        CreateMap<ImportRequest, ImportCRUDDTO>()
            .ForMember(src => src.RequestStatus, opt => opt.MapFrom(_ => _.RequestStatus.ToString()));
        CreateMap<ExportRequest, ExportRequestCrudDTO>()
            .ForMember(src => src.RequestStatus, opt => opt.MapFrom(src => src.RequestStatus.ToString()));
    }
}
