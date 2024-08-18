using AgencyApi.AppointmentModule.Models;

namespace AgencyApi.AppointmentModule.Services;

public interface IAppointmentService
{
    List<Appointment> GetAppointments(int agencyId, DateTime date);
    void AddAppointment(Appointment appointment);
    void UpdateAppointment(Appointment appointment);
    void DeleteAppointment(int id);
    Appointment? GetAppointment(int id);
    List<Appointment> GetAppointmentsByCustomer(int customerId, DateTime date);
    long GetCountOfAppointmentsOnDate(int agencyId, DateTime date);
    bool IsFull(int appointmentAgencyId, DateTime appointmentDate);
}