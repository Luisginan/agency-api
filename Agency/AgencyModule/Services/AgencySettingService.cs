using Agency.AgencyModule.Repos;
using Agency.AppointmentModule.Services;

namespace Agency.AgencyModule.Services;

public class AgencySettingService(IAgencySettingRepository agencySettingRepository, IAppointmentService appointmentService) : IAgencySettingService
{
    public void AddHoliday(int agencyId, DateTime date)
    {
        agencySettingRepository.AddHoliday(agencyId, date);
    }

    public void RemoveHoliday(int agencyId, DateTime date)
    {
        agencySettingRepository.RemoveHoliday(agencyId, date);
    }

    public void AddMaxAppointmentsPerDay(int agencyId, int maxAppointments)
    {
        agencySettingRepository.AddMaxAppointmentsPerDay(agencyId, maxAppointments);
    }

    public void RemoveMaxAppointmentsPerDay(int agencyId)
    {
       agencySettingRepository.AddMaxAppointmentsPerDay(agencyId, 0);
    }

    public int GetMaxAppointmentsPerDay(int agencyId)
    {
        return agencySettingRepository.GetMaxAppointmentsPerDay(agencyId);
    }

    public int GetCountOfAppointmentsOnDate(int agencyId, DateTime date)
    {
       return appointmentService.GetCountOfAppointmentsOnDate(agencyId, date);
    }

    public bool IsHoliday(int agencyId, DateTime date)
    {
        return agencySettingRepository.IsHoliday(agencyId, date);
    }

    public bool IsFull(int appointmentAgencyId, DateTime appointmentDate)
    {
        var maxAppointments = GetMaxAppointmentsPerDay(appointmentAgencyId);
        var countOfAppointments = GetCountOfAppointmentsOnDate(appointmentAgencyId, appointmentDate);
        return countOfAppointments >= maxAppointments;
    }
}