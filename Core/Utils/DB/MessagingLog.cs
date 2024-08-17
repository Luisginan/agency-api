using System.Diagnostics.CodeAnalysis;

namespace Core.Utils.DB;

[ExcludeFromCodeCoverage]
[Table("messaging_log","id")]
public class MessagingLog
{
    [Field("id")] public string Id { get; set; } = "";
    [Field("topic")]
    public string Topic { get; init; } = "";
    [Field("group_id")]
    public string GroupId { get; init; } = "";
    [Field("app_id")]
    public string? AppId { get; init; } = "";
    [Field("key")]
    public string Key { get; init; } = "";
    [Field("payload")]
    public string PayLoad { get; init; } = "";
    [Field("retry")]
    public int Retry { get; init; }
    [Field("method")]
    public string Method { get; init; } = "";

    [Field("error")] public string Error { get; init; } = "";
    
}