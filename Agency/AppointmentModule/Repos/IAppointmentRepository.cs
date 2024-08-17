using Agency.AppointmentModule.Models;

namespace Agency.AppointmentModule.Repos;

public interface IAppointmentRepository
{
    List<Appointment> GetAppointments(int agencyId, DateTime date);
    int AddAppointment(Appointment appointment);
    void UpdateAppointment(Appointment appointment);
    Appointment? GetAppointment(int id);
    void DeleteAppointment(int id);
    List<Appointment> GetAppointmentsByCustomer(int customerId, DateTime date);
    int GetCountOfAppointmentsOnDate(int agencyId, DateTime date);
}