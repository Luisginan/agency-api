using Agency.AppointmentModule.Models;
using Core.Base;
using Core.CExceptions;
using Core.Utils.DB;

namespace Agency.AppointmentModule.Repos;

public class AppointmentRepository (INawaDaoRepository nawaDaoRepository, IQueryBuilderRepository queryBuilderRepository):
    DalBase<Appointment>(nawaDaoRepository,queryBuilderRepository),IAppointmentRepository
{
   
    public List<Appointment> GetAppointments(int agencyId, DateTime date)
    {
        var listAppointment = NawaDao.ExecuteTable<Appointment>("SELECT * FROM appointment WHERE agency_id = @agencyId AND date = @date", new List<FieldParameter>
        {
            new("agencyId", agencyId),
            new("date", date)
        });

        return listAppointment;
    }

    public int AddAppointment(Appointment appointment)
    {
        
        var id = NawaDao.ExecuteScalar("INSERT INTO appointment (date, description, agency_id, customer_id) VALUES (@date, @description, @agencyId, @customerId) returning id", new List<FieldParameter>
        {
            new("date", appointment.Date),
            new("description", appointment.Description),
            new("agencyId", appointment.AgencyId),
            new("customerId", appointment.CustomerId)
        });
        
        if (id == null)
        {
            throw new RepositoryException("Failed to add appointment");
        }

        return (int)id;
    }

    public void UpdateAppointment(Appointment appointment)
    {
        var impacted = NawaDao.ExecuteNonQuery("UPDATE appointment SET date = @date, description = @description, agency_id = @agencyId, customer_id = @customerId WHERE id = @id", new List<FieldParameter>
        {
            new("date", appointment.Date),
            new("description", appointment.Description),
            new("agencyId", appointment.AgencyId),
            new("customerId", appointment.CustomerId),
            new("id", appointment.Id)
        });
        
        if (impacted == 0)
        {
            throw new RepositoryException("Failed to update appointment");
        }
    }

    public Appointment? GetAppointment(int id)
    {
        return Get(id);
    }

    public void DeleteAppointment(int id)
    {
        var impacted = NawaDao.ExecuteNonQuery("DELETE FROM appointment WHERE id = @id", new List<FieldParameter>
        {
            new("id", id)
        });
        
        if (impacted == 0)
        {
            throw new RepositoryException("Failed to delete appointment");
        }
    }

    public List<Appointment> GetAppointmentsByCustomer(int customerId, DateTime date)
    {
        var listAppointment = NawaDao.ExecuteTable<Appointment>("SELECT * FROM appointment WHERE customer_id = @customerId AND date = @date", new List<FieldParameter>
        {
            new("customerId", customerId),
            new("date", date)
        });

        return listAppointment;
    }
    
    public int GetCountOfAppointmentsOnDate(int agencyId, DateTime date)
    {
        var countOfAppointments = NawaDao.ExecuteScalar("SELECT COUNT(*) FROM appointment WHERE agency_id = @agencyId AND date = @date", new List<FieldParameter>
        {
            new("agencyId", agencyId),
            new("date", date)
        });

        return (int)countOfAppointments;
    }
}