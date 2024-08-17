namespace Agency.AgencyModule.Services;

public interface IAgencySettingService
{
    void AddHoliday(int agencyId, DateTime date);
    void RemoveHoliday(int agencyId, DateTime date);
    void AddMaxAppointmentsPerDay(int agencyId, int maxAppointments);
    void RemoveMaxAppointmentsPerDay(int agencyId);
    int GetMaxAppointmentsPerDay(int agencyId);
    int GetCountOfAppointmentsOnDate(int agencyId, DateTime date);
    bool IsHoliday(int agencyId, DateTime date);
    bool IsFull(int appointmentAgencyId, DateTime appointmentDate);
}