using Core.Utils.DB;

namespace AgencyApi.AppointmentModule.Models;
[Table("appointment", "id")]
public class Appointment
{
    [Field("id")]
    public int Id { get; set; }
    [Field("date")]
    public DateTime Date { get; set; }
    [Field("description")]
    public string Description { get; set; } = string.Empty;
    [Field("agency_id")]
    public int AgencyId { get; set; }
    [Field("customer_id")]
    public int CustomerId { get; set; }
}