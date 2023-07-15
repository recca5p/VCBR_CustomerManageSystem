using AutoMapper;
using VCBRDemo.Customers;
using VCBRDemo.Customers.DTOs;

namespace VCBRDemo;

public class VCBRDemoApplicationAutoMapperProfile : Profile
{
    public VCBRDemoApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Customer, CustomerDTO>();
        CreateMap<CustomerCreateDTO, Customer>();
    }
}
