using System.Text.Json.Serialization;
using Agency.AgencyModule.Dto;
using Agency.CustomerModule.DTO;

namespace Agency.AppointmentModule.Dto;

public class AppointmentResponseDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("agencyId")]
    public int AgencyId { get; set; }
    [JsonPropertyName("customerId")]
    public int CustomerId { get; set; }
    [JsonPropertyName("agency")]
    public AgencyResponseDto Agency { get; set; } = new();
    [JsonPropertyName("customer")]
    public CustomerResponse Customer { get; set; } = new();
}