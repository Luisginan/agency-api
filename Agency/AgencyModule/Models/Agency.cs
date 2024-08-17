using Core.Utils.DB;

namespace Agency.AgencyModule.Models;
[Table("agency", "id")]
public class Agency
{
    [Field("id")]
    public int Id { get; set; }
    [Field("name")]
    public string Name { get; set; }
    [Field("address")]
    public string Address { get; set; }
    [Field("city")]
    public string City { get; set; }
}