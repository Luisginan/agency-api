using AgencyApi.AgencyModule.Repos;

namespace AgencyApi.AgencyModule.Services;

public class AgencySettingService(IAgencySettingRepository agencySettingRepository) : IAgencySettingService
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
    
    public bool IsHoliday(int agencyId, DateTime date)
    {
        return agencySettingRepository.IsHoliday(agencyId, date);
    }

    
}