using AgencyApi.AgencyModule.Models;
using Core.Base;
using Core.CExceptions;
using Core.Utils.DB;

namespace AgencyApi.AgencyModule.Repos;

public class AgencySettingRepository (INawaDaoRepository nawaDaoRepository, IQueryBuilderRepository queryBuilderRepository) : 
    DalBase<AgencySetting>(nawaDaoRepository, queryBuilderRepository), IAgencySettingRepository
{
    public int AddHoliday(int agencyId, DateTime date)
    {
        var id = (int) NawaDao.ExecuteScalar("INSERT INTO agency_holiday (agency_id, holiday) VALUES (@agencyId, @date) returning id", new List<FieldParameter>
        {
            new("agencyId", agencyId),
            new("date", date)
        });

        return id;
    }

    public void RemoveHoliday(int agencyId, DateTime date)
    {
        
        var impacted = NawaDao.ExecuteNonQuery("DELETE FROM agency_holiday WHERE agency_id = @agencyId AND holiday = @date", new List<FieldParameter>
        {
            new("agencyId", agencyId),
            new("date", date)
        });
        
        if (impacted == 0)
        {
            throw new RepositoryException("Failed to remove holiday");
        }
    }

    public void AddMaxAppointmentsPerDay(int agencyId, int maxAppointments)
    {
        var impacted = NawaDao.ExecuteNonQuery("UPDATE agency_settings SET max_appointments_per_day = @maxAppointments WHERE agency_id = @agencyId", new List<FieldParameter>
        {
            new("maxAppointments", maxAppointments),
            new("agencyId", agencyId)
        });
        
        if (impacted == 0)
        {
            throw new RepositoryException("Failed to add max appointments per day");
        }
    }

    public int GetMaxAppointmentsPerDay(int agencyId)
    {
        var maxAppointments = NawaDao.ExecuteScalar("SELECT max_appointments_per_day FROM agency_settings WHERE agency_id = @agencyId", new List<FieldParameter>
        {
            new("agencyId", agencyId)
        });

        return (int)maxAppointments;
    }

    public bool IsHoliday(int agencyId, DateTime date)
    {
        var isHoliday = NawaDao.ExecuteScalar("SELECT COUNT(*) FROM agency_holiday WHERE agency_id = @agencyId AND holiday = @date", new List<FieldParameter>
        {
            new("agencyId", agencyId),
            new("date", date)
        });

        return (Int64)isHoliday > 0;
    }

    
}