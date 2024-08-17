using System.Diagnostics.CodeAnalysis;
using Agency.CustomerModule.DTO;
using AutoMapper;

namespace Agency.CustomerModule.Models;
[ExcludeFromCodeCoverage]
public class CustomerMapper: Profile
{
    
    public CustomerMapper()
    {
        CreateMap<Customer, CustomerResponse>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email2));
        CreateMap<Customer, CustomerRequest>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email2));
        CreateMap<CustomerResponse, Customer>()
            .ForMember(dest => dest.Email2, opt => opt.MapFrom(src => src.Email));
        CreateMap<CustomerRequest, Customer>()
            .ForMember(dest => dest.Email2, opt => opt.MapFrom(src => src.Email));
    }
}