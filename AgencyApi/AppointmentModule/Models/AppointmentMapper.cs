using AgencyApi.AppointmentModule.Dto;
using AutoMapper;

namespace AgencyApi.AppointmentModule.Models;

public class AppointmentMapper : Profile
{
    public AppointmentMapper()
    {
        CreateMap<Appointment, AppointmentRequestDto>();
        CreateMap<AppointmentRequestDto, Appointment>();
    }
}