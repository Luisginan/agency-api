using AgencyApi.AgencyModule.Repos;
using Core.Utils.DB;

namespace AgencyTest.AgencyModule.Repos;

public class AgencySettingRepositoryTest :  BaseRepoTest
{
    [Fact]
    public void GetAgencySettingTest()
    {
        // arrange
        var idHoliday = NawaDao.ExecuteScalar("INSERT INTO agency_holiday (agency_id, holiday) VALUES (@agencyId, @holiday) returning id", new List<FieldParameter>
        {
            new("agencyId", 1),
            new("holiday", new DateTime(2021, 12, 25))
        });
        
        // act
        var agencySettingRepository = new AgencySettingRepository(NawaDao, QueryBuilder);
        var isHoliday = agencySettingRepository.IsHoliday(1, new DateTime(2021, 12, 25));
        
        // assert
        Assert.True(isHoliday);
        
        // cleaning
        NawaDao.ExecuteNonQuery("DELETE FROM agency_holiday WHERE id = @id", new List<FieldParameter>
        {
            new("id", idHoliday)
        });
    }
    
    [Fact]
    public void AddHolidayTest()
    {
        // arrange
        var agencySettingRepository = new AgencySettingRepository(NawaDao, QueryBuilder);
        // act
        var idHoliday = agencySettingRepository.AddHoliday(1, new DateTime(2021, 12, 25));
        // assert
        Assert.True(idHoliday > 0);
        // cleaning
        NawaDao.ExecuteNonQuery("DELETE FROM agency_holiday WHERE id = @id", new List<FieldParameter>
        {
            new("id", idHoliday)
        });
    }
    
    [Fact]
    public void RemoveHolidayTest()
    {
        // arrange
        var agencySettingRepository = new AgencySettingRepository(NawaDao, QueryBuilder);
        
        var idHoliday = (int) NawaDao.ExecuteScalar("INSERT INTO agency_holiday (agency_id, holiday) VALUES (@agencyId, @holiday) returning id", new List<FieldParameter>
        {
            new("agencyId", 1),
            new("holiday", new DateTime(2021, 12, 25))
        });
        
        // act
        agencySettingRepository.RemoveHoliday(1, new DateTime(2021, 12, 25));
        
        
        // assert
        var holiday = (long) NawaDao.ExecuteScalar("SELECT COUNT(*) FROM agency_holiday WHERE agency_id = @agencyId AND holiday = @holiday", new List<FieldParameter>
        {
            new("agencyId", 1),
            new("holiday", new DateTime(2021, 12, 25))
        });
        Assert.Equal(0, holiday);
        
        // cleaning
        NawaDao.ExecuteNonQuery("DELETE FROM agency_holiday WHERE id = @id", new List<FieldParameter>
        {
            new("id", idHoliday)
        });
    }
    
    [Fact]
    public void AddMaxAppointmentsPerDayTest()
    {
        // arrange
        var id = (int) NawaDao.ExecuteScalar("INSERT INTO agency_settings (agency_id, max_appointments_per_day) VALUES (@agencyId, @maxAppointments) returning id", new List<FieldParameter>
        {
            new("agencyId", 1),
            new("maxAppointments", 0)
        });
        
        var agencySettingRepository = new AgencySettingRepository(NawaDao, QueryBuilder);

        // act
        agencySettingRepository.AddMaxAppointmentsPerDay(1, 20);
        
        // assert
        var maxAppointments = (int) NawaDao.ExecuteScalar("SELECT max_appointments_per_day FROM agency_settings WHERE agency_id = @agencyId", new List<FieldParameter>
        {
            new("agencyId", 1)
        });
        
        Assert.Equal(20, maxAppointments);
        
        //cleaning
        NawaDao.ExecuteNonQuery("DELETE FROM agency_settings WHERE id = @id", new List<FieldParameter>
        {
            new("id", id)
        });
    }
    
    [Fact]
    public void GetMaxAppointmentsPerDayTest()
    {
        // arrange
        var agencySettingRepository = new AgencySettingRepository(NawaDao, QueryBuilder);
        var id = (int) NawaDao.ExecuteScalar("INSERT INTO agency_settings (agency_id, max_appointments_per_day) VALUES (@agencyId, @maxAppointments) returning id", new List<FieldParameter>
        {
            new("agencyId", 1),
            new("maxAppointments", 20)
        });
        
        // act
        var maxAppointments = agencySettingRepository.GetMaxAppointmentsPerDay(1);
        Assert.Equal(20, maxAppointments);
        
        //cleaning
        NawaDao.ExecuteNonQuery("DELETE FROM agency_settings WHERE id = @id", new List<FieldParameter>
        {
            new("id", id)
        });
    }
    
}