

using Core.Utils.DB;

namespace AgencyApi.TokenIssuanceModule.Models;
[Table("token_issuance", "id")]
public class TokenIssuance
{
    [Field("id")]
    public int Id { get; set; }
    [Field("token")]
    public string Token { get; set; } = string.Empty;
    [Field("issuance_date")]
    public DateTime IssuanceDate { get; set; }
    [Field("expiry_date")]
    public DateTime ExpiryDate { get; set; }
    [Field("customer_id")]
    public int CustomerId { get; set; } 
    [Field("agency_id")]
    public int AgencyId { get; set; } 
    [Field("appointment_id")]
    public int AppointmentId { get; set; }
}