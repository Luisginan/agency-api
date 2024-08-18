using System.Text.Json.Serialization;

namespace AgencyApi.AgencyModule.Dto;

public class AgencyRequestDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("address")]
    public string Address { get; set; }
    [JsonPropertyName("city")]
    public string City { get; set; }
}