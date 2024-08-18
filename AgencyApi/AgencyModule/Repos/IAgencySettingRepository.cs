namespace AgencyApi.AgencyModule.Repos;

public interface IAgencySettingRepository
{
    int AddHoliday(int agencyId, DateTime date);
    void RemoveHoliday(int agencyId, DateTime date);
    void AddMaxAppointmentsPerDay(int agencyId, int maxAppointments);
    int GetMaxAppointmentsPerDay(int agencyId);
    bool IsHoliday(int agencyId, DateTime date);
}