using System.Diagnostics.CodeAnalysis;

namespace Core.Config;

[ExcludeFromCodeCoverage]
public class MessagingConfig
{
    public string SaslUsername { get; init; } = "";
    public string SaslPassword { get; init; } = "";
    public int SessionTimeoutMs { get; init; }
    public string BootstrapServers { get; init; } = "";

    public bool Authentication { get; init; }

    public string ProjectId { get; init; } = "";
    public string Provider { get; init; } = "pubsub";
    public string TopicSuffix { get; init; } = "";
}