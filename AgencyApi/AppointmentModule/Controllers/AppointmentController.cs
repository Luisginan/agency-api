using AgencyApi.AgencyModule.Dto;
using AgencyApi.AgencyModule.Services;
using AgencyApi.AppointmentModule.Dto;
using AgencyApi.AppointmentModule.Models;
using AgencyApi.AppointmentModule.Services;
using AgencyApi.CustomerModule.DTO;
using AgencyApi.CustomerModule.Services;
using AutoMapper;
using Core.Base;
using Core.Utils.DB;
using Microsoft.AspNetCore.Mvc;

namespace AgencyApi.AppointmentModule.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AppointmentController(
    ICache cache, 
    ILogger<AppointmentController> logger, 
    IAppointmentService appointmentService, 
    IMapper mapper,
    ICustomerService customerService,
    IAgencyService agencyService,
    IConnection dbConnection) : SuperController(cache, logger, dbConnection)
{
    protected override string CacheKeyRoot => "AgencyApi.AppointmentModule.Controllers.AppointmentController";
    
    [HttpPost]
    public async Task<IActionResult> AddAppointment([FromBody] AppointmentRequestDto? appointment)
    {
        if (appointment == null)
            return await Task.FromResult<IActionResult>(BadRequest());
        
        var appointmentModel = mapper.Map<Appointment>(appointment);
        
        await UseDbTransaction(() =>
        {
            appointmentService.AddAppointment(appointmentModel);
            return Task.CompletedTask;
        });
        
        return await Task.FromResult<IActionResult>(Ok());
    }
    
    [HttpPut]
    public Task<IActionResult> UpdateAppointment([FromBody] AppointmentRequestDto? appointment)
    {
        if (appointment == null)
            return Task.FromResult<IActionResult>(BadRequest());
        var appointmentModel = mapper.Map<Appointment>(appointment);
        appointmentService.UpdateAppointment(appointmentModel);
        return Task.FromResult<IActionResult>(Ok());
    }
    
    [HttpDelete("{id:int}")]
    public Task<IActionResult> DeleteAppointment(int id)
    {
        appointmentService.DeleteAppointment(id);
        return Task.FromResult<IActionResult>(Ok());
    }
    
    [HttpGet("{agencyId:int}/{date:DateTime}")]
    public Task<IActionResult> GetAppointments(int agencyId, DateTime date)
    {
        var appointmentResponse = UseListCache("GetAppointments.UseListCache", () =>
        {
            var appointments = appointmentService.GetAppointments(agencyId, date);
            var appointmentResponse = mapper.Map<List<AppointmentResponseDto>>(appointments);
            appointmentResponse.ForEach(appointment =>
            {
                appointment.Agency = mapper.Map<AgencyResponseDto>(agencyService.GetAgency(appointment.AgencyId));
                appointment.Customer = mapper.Map<CustomerResponse>(customerService.GetCustomer(appointment.CustomerId));
            });
            return appointmentResponse;
        });
        
        return Task.FromResult<IActionResult>(Ok(appointmentResponse));
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAppointment(int id)
    {
        var appointmentResponse = await UseCacheAsync("GetAppointment.UseListCacheAsync", () =>
        {
            var appointment = appointmentService.GetAppointment(id);
            if (appointment == null)
                return Task.FromResult<AppointmentResponseDto?>(null);

            var appointmentResponse = mapper.Map<AppointmentResponseDto>(appointment);
            appointmentResponse.Agency = mapper.Map<AgencyResponseDto>(agencyService.GetAgency(appointment.AgencyId));
            appointmentResponse.Customer = mapper.Map<CustomerResponse>(customerService.GetCustomer(appointment.CustomerId));
            return Task.FromResult<AppointmentResponseDto?>(appointmentResponse);
        });
       
        return await Task.FromResult<IActionResult>(Ok(appointmentResponse));
    }
    
    [HttpGet("GetAppointmentsByCustomer")]
    public async Task<IActionResult> GetAppointmentsByCustomer([FromQuery] int customerId, DateTime date)
    {
        var appointmentResponse = await UseListCacheAsync("GetAppointmentsByCustomer.UseListCacheAsync", () =>
        {
            var appointments = appointmentService.GetAppointmentsByCustomer(customerId, date);
            var appointmentResponse = mapper.Map<List<AppointmentResponseDto>>(appointments);
            appointmentResponse.ForEach(appointment =>
            {
                appointment.Agency = mapper.Map<AgencyResponseDto>(agencyService.GetAgency(appointment.AgencyId));
                appointment.Customer = mapper.Map<CustomerResponse>(customerService.GetCustomer(appointment.CustomerId));
            });
            return Task.FromResult(appointmentResponse);
        }); 
        
        return await Task.FromResult<IActionResult>(Ok(appointmentResponse));
    }
}