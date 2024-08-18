using AgencyApi.AppointmentModule.Models;
using AgencyApi.AppointmentModule.Repos;
using Core.Utils.DB;

namespace AgencyTest.AppointmentModule.Repos;

public class AppointmentRepositoryTest : BaseRepoTest
{
    [Fact]
    public void GetAppointmentsTest()
    {
        var id = (int) NawaDao.ExecuteScalar("INSERT INTO appointment (date, description, agency_id, customer_id) VALUES (@date, @description, @agencyId, @customerId) returning id", new List<FieldParameter>
        {
            new("date", new DateTime(2024, 8, 17)),
            new("description", "Join Company"),
            new("agencyId", 1),
            new("customerId", 1)
        });
        
        var appointmentRepository = new AppointmentRepository(NawaDao, QueryBuilder);
        var appointments = appointmentRepository.GetAppointment(id);
   
        Assert.NotNull(appointments);
        Assert.Equal("Join Company", appointments.Description);
        
        //cleaning
        NawaDao.ExecuteNonQuery("DELETE FROM appointment WHERE id = @id", new List<FieldParameter>
        {
            new("id", id)
        });
    }
    
    [Fact]
    public void AddAppointmentTest()
    {
        var appointmentRepository = new AppointmentRepository(NawaDao, QueryBuilder);
        var appointment = new Appointment
        {
            AgencyId = 1,
            CustomerId = 1,
            Date = new DateTime(2024, 8, 20),
            Description = "Join Company",
        };
        
        var id = appointmentRepository.AddAppointment(appointment);
        
        Assert.True(id  > 0);
        
        // cleaning
        NawaDao.ExecuteNonQuery("DELETE FROM appointment WHERE id = @id", new List<FieldParameter>
        {
            new("id", id)
        });
    }
    
    [Fact]
    public void UpdateAppointmentTest()
    {
        var appointmentRepository = new AppointmentRepository(NawaDao, QueryBuilder);
        var appointment = new Appointment
        {
            AgencyId = 1,
            CustomerId = 1,
            Date = new DateTime(2024, 8, 21),
            Description = "Join Company xxx",
        };
        
        var id = NawaDao.ExecuteScalar("INSERT INTO appointment (date, description, agency_id, customer_id) VALUES (@date, @description, @agencyId, @customerId) returning id", new List<FieldParameter>
        {
            new("date", appointment.Date),
            new("description", appointment.Description),
            new("agencyId", appointment.AgencyId),
            new("customerId", appointment.CustomerId)
        });
        
        appointment.Id = (int)id;
        appointment.Description = "Join Company yyy";
        
        appointmentRepository.UpdateAppointment(appointment);
        
        var updatedAppointment = NawaDao.ExecuteRow<Appointment>("SELECT * FROM appointment WHERE id = @id", new List<FieldParameter>
        {
            new("id", id)
        });
        
        Assert.NotNull(updatedAppointment);
        Assert.Equal("Join Company yyy", updatedAppointment.Description);
        
        //cleaning
        NawaDao.ExecuteNonQuery("DELETE FROM appointment WHERE id = @id", new List<FieldParameter>
        {
            new("id", id)
        });
    }
    
    [Fact]
    public void DeleteAppointmentTest()
    {
        var appointmentRepository = new AppointmentRepository(NawaDao, QueryBuilder);
        
        var appointment = new Appointment
        {
            AgencyId = 1,
            CustomerId = 1,
            Date = new DateTime(2024, 8, 25),
            Description = "Join Mafia",
        };
        
        var id = (int) NawaDao.ExecuteScalar("INSERT INTO appointment (date, description, agency_id, customer_id) VALUES (@date, @description, @agencyId, @customerId) returning id", new List<FieldParameter>
        {
            new("date", appointment.Date),
            new("description", appointment.Description),
            new("agencyId", appointment.AgencyId),
            new("customerId", appointment.CustomerId)
        });
        
        appointmentRepository.DeleteAppointment(id);
        
        var appointments = NawaDao.ExecuteRow<Appointment>("SELECT * FROM appointment WHERE id = @id", new List<FieldParameter>
        {
            new("id", id)
        });
        
        Assert.Null(appointments);
    }
    
    [Fact]
    public void GetAppointmentTest()
    {
        var appointmentRepository = new AppointmentRepository(NawaDao, QueryBuilder);

        for (int i = 0; i < 4; i++)
        {
            var appointment = new Appointment
            {
                AgencyId = 1,
                CustomerId = 1,
                Date = new DateTime(2024, 9, 25),
                Description = "Join Mafia",
            };
        
            NawaDao.ExecuteScalar("INSERT INTO appointment (date, description, agency_id, customer_id) VALUES (@date, @description, @agencyId, @customerId) returning id", new List<FieldParameter>
            {
                new("date", appointment.Date),
                new("description", appointment.Description),
                new("agencyId", appointment.AgencyId),
                new("customerId", appointment.CustomerId)
            });
        }
        
        
        var appointments = appointmentRepository.GetAppointments(1, new DateTime(2024, 9, 25));
        
        Assert.Equal(4, appointments.Count);
        
        //cleaning
        NawaDao.ExecuteNonQuery("DELETE FROM appointment WHERE date = @date and agency_id = @agencyId", new List<FieldParameter>
        {
            new("date", new DateTime(2024, 9, 25)),
            new("agencyId", 1)
        });
    }
    
    [Fact]
    public void GetAppointmentByCustomerTest()
    {
        var appointmentRepository = new AppointmentRepository(NawaDao, QueryBuilder);

        for (int i = 0; i < 4; i++)
        {
            var appointment = new Appointment
            {
                AgencyId = 1,
                CustomerId = 1,
                Date = new DateTime(2024, 10, 25),
                Description = "Join Mafia",
            };
        
            NawaDao.ExecuteScalar("INSERT INTO appointment (date, description, agency_id, customer_id) VALUES (@date, @description, @agencyId, @customerId) returning id", new List<FieldParameter>
            {
                new("date", appointment.Date),
                new("description", appointment.Description),
                new("agencyId", appointment.AgencyId),
                new("customerId", appointment.CustomerId)
            });
        }
        
        
        var appointments = appointmentRepository.GetAppointmentsByCustomer(1, new DateTime(2024, 10, 25));
        
        Assert.Equal(4, appointments.Count);
        
        //cleaning
        NawaDao.ExecuteNonQuery("DELETE FROM appointment WHERE date = @date and customer_id = @customerId", new List<FieldParameter>
        {
            new("date", new DateTime(2024, 10, 25)),
            new("customerId", 1)
        });
    }
    
    
}