
using Core.Utils.DB;
namespace Agency.AgencyModule.Models;

[Table("agency_setting", "id")]
public class AgencySetting
{
    [Field("id")]
    public int Id { get; set; }
    [Field("agency_id")]
    public int AgencyId { get; set; }
    [Field("max_appointments_per_day")]
    public int MaxAppointmentsPerDay { get; set; }
}