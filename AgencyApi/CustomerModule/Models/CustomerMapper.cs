﻿using System.Diagnostics.CodeAnalysis;
using AgencyApi.CustomerModule.DTO;
using AutoMapper;

namespace AgencyApi.CustomerModule.Models;
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