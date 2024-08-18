using System.Diagnostics.CodeAnalysis;

namespace AgencyApi.CustomerModule.Models;

[ExcludeFromCodeCoverage]
public class CustomerPayload
{
    public string? Id { get; init; }
    public string? Message { get; init; }
}