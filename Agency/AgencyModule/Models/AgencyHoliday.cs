

using Core.Utils.DB;

namespace Agency.AgencyModule.Models;
[Table("agency_holiday", "id")]
public class AgencyHoliday
{
    [Field("id")]
    public int Id { get; set; }
    [Field("agency_id")]
    public int AgencyId { get; set; }
    [Field("holiday")]
    public DateTime Holiday { get; set; }
}