using Agency.AgencyModule.Services;
using Agency.AppointmentModule.Models;
using Agency.AppointmentModule.Repos;
using Agency.CustomerModule.Services;
using Agency.TokenIssuanceModule.Models;
using Agency.TokenIssuanceModule.Services;
using Core.CExceptions;
using Core.Utils.DB;

namespace Agency.AppointmentModule.Services;

public class AppointmentService (
    IAppointmentRepository appointmentRepository,
    IAgencySettingService agencySettingService,
    IAgencyService agencyService,
    ICustomerService customerService,
    ITokenIssuanceService tokenIssuanceService,
    IConnection connection
    ): IAppointmentService
{
    public List<Appointment> GetAppointments(int agencyId, DateTime date)
    {
        var agency = agencyService.GetAgency(agencyId);
        if (agency == null)
        {
            throw new RepositoryException("Agency not found");
        }
        
        return appointmentRepository.GetAppointments(agencyId, date);
    }

    public void AddAppointment(Appointment appointment)
    {
        
        var agency = agencyService.GetAgency(appointment.AgencyId);
        if (agency == null)
        {
            throw new RepositoryException("Agency not found");
        }
        
        while (agencySettingService.IsHoliday(appointment.AgencyId, appointment.Date))
        {
            appointment.Date = appointment.Date.AddDays(1);
        }

        while (agencySettingService.IsFull(appointment.AgencyId, appointment.Date))
        {
            appointment.Date = appointment.Date.AddDays(1);
        }

        try
        {
            connection.BeginTransaction();
            appointmentRepository.AddAppointment(appointment);
            var token = new TokenIssuance()
            {
                ExpiryDate = appointment.Date,
                Token = new Guid().ToString(),
                IssuanceDate = appointment.Date,
                CustomerId = appointment.CustomerId,
                AgencyId = appointment.AgencyId,
                AppointmentId = appointment.Id
            };
            tokenIssuanceService.AddTokenIssuance(token);
            connection.Commit();
        }
        catch (Exception)
        {
            connection.Rollback();
            throw;
        }
       
    }

    public void UpdateAppointment(Appointment appointment)
    {
        var agency = agencyService.GetAgency(appointment.AgencyId);
        if (agency == null)
        {
            throw new RepositoryException("Agency not found");
        }
        
        while (agencySettingService.IsHoliday(appointment.AgencyId, appointment.Date))
        {
            appointment.Date = appointment.Date.AddDays(1);
        }

        while (agencySettingService.IsFull(appointment.AgencyId, appointment.Date))
        {
            appointment.Date = appointment.Date.AddDays(1);
        }
        
        appointmentRepository.UpdateAppointment(appointment);
    }

    public void DeleteAppointment(int id)
    {
        var appointment = appointmentRepository.GetAppointment(id);
        if (appointment == null)
        {
            throw new RepositoryException("Appointment not found");
        }
        
        appointmentRepository.DeleteAppointment(id);
    }

    public Appointment? GetAppointment(int id)
    {
        return appointmentRepository.GetAppointment(id);
    }

    public List<Appointment> GetAppointmentsByCustomer(int customerId, DateTime date)
    {
        var customer = customerService.GetCustomer(customerId);
        if (customer == null)
        {
            throw new RepositoryException("Customer not found");
        }
        
        return appointmentRepository.GetAppointmentsByCustomer(customerId, date);
    }

    public int GetCountOfAppointmentsOnDate(int agencyId, DateTime date)
    {
        var agency = agencyService.GetAgency(agencyId);
        if (agency == null)
        {
            throw new RepositoryException("Agency not found");
        }
        
        return appointmentRepository.GetCountOfAppointmentsOnDate(agencyId, date);
    }
}