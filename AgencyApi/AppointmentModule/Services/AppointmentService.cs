using AgencyApi.AgencyModule.Services;
using AgencyApi.AppointmentModule.Models;
using AgencyApi.AppointmentModule.Repos;
using AgencyApi.CustomerModule.Services;
using AgencyApi.TokenIssuanceModule.Models;
using AgencyApi.TokenIssuanceModule.Services;
using Core.CExceptions;


namespace AgencyApi.AppointmentModule.Services;

public class AppointmentService (
    IAppointmentRepository appointmentRepository,
    IAgencySettingService agencySettingService,
    ICustomerService customerService,
    IAgencyService agencyService,
    ITokenIssuanceService tokenIssuanceService
    ): IAppointmentService
{
    public List<Appointment> GetAppointments(int agencyId, DateTime date)
    {
        var agency = agencyService.GetAgency(agencyId);
        if (agency == null)
            throw new RepositoryException("AgencyApi not found");
        
        return appointmentRepository.GetAppointments(agencyId, date);
    }

    public void AddAppointment(Appointment appointment)
    {
        
        var agency = agencyService.GetAgency(appointment.AgencyId);
        if (agency == null)
        {
            throw new RepositoryException("Agency not found");
        }
        
        var customer = customerService.GetCustomer(appointment.CustomerId);
        if (customer == null)
        {
            throw new RepositoryException("Customer not found");
        }
        
        while (agencySettingService.IsHoliday(appointment.AgencyId, appointment.Date))
        {
            appointment.Date = appointment.Date.AddDays(1);
        }

        while (IsFull(appointment.AgencyId, appointment.Date))
        {
            appointment.Date = appointment.Date.AddDays(1);
        }
        
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
    }

    public void UpdateAppointment(Appointment appointment)
    {
        var agency = agencyService.GetAgency(appointment.AgencyId);
        if (agency == null)
        {
            throw new RepositoryException("AgencyApi not found");
        }
        
        var customer = customerService.GetCustomer(appointment.CustomerId);
        if (customer == null)
        {
            throw new RepositoryException("Customer not found");
        }
        
        while (agencySettingService.IsHoliday(appointment.AgencyId, appointment.Date))
        {
            appointment.Date = appointment.Date.AddDays(1);
        }

        while (IsFull(appointment.AgencyId, appointment.Date))
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

    public long GetCountOfAppointmentsOnDate(int agencyId, DateTime date)
    {
        var agency = agencyService.GetAgency(agencyId);
        if (agency == null)
        {
            throw new RepositoryException("Agency not found");
        }
        
        return appointmentRepository.GetCountOfAppointmentsOnDate(agencyId, date);
    }
    
    public bool IsFull(int appointmentAgencyId, DateTime appointmentDate)
    {
        var maxAppointments = agencySettingService.GetMaxAppointmentsPerDay(appointmentAgencyId);
        var countOfAppointments = GetCountOfAppointmentsOnDate(appointmentAgencyId, appointmentDate);
        if (maxAppointments == 0)
            return false;
        
        return countOfAppointments >= maxAppointments;
    }
}