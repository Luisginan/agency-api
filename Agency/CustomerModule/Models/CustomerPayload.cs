using System.Diagnostics.CodeAnalysis;

namespace Agency.CustomerModule.Models;

[ExcludeFromCodeCoverage]
public class CustomerPayload
{
    public string? Id { get; init; }
    public string? Message { get; init; }
}