namespace AgencyApi.AgencyModule.Services;

public interface IAgencySettingService
{
    void AddHoliday(int agencyId, DateTime date);
    void RemoveHoliday(int agencyId, DateTime date);
    void AddMaxAppointmentsPerDay(int agencyId, int maxAppointments);
    void RemoveMaxAppointmentsPerDay(int agencyId);
    int GetMaxAppointmentsPerDay(int agencyId);
    bool IsHoliday(int agencyId, DateTime date);
   
}