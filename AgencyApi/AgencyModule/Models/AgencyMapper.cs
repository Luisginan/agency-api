using AgencyApi.AgencyModule.Dto;
using AutoMapper;

namespace AgencyApi.AgencyModule.Models;

public class AgencyMapper : Profile
{
    public AgencyMapper()
    {
        CreateMap<Agency, AgencyRequestDto>();
        CreateMap<AgencyRequestDto, Agency>();
        CreateMap<Agency, AgencyResponseDto>();
    }
}