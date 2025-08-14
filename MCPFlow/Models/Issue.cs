using System.Text.Json.Serialization;

namespace MCPFlow;
public class Issue
{
    // Machine-readable code (e.g., NOT_FOUND, REQUIRED, TYPE, PERMISSION)
    [JsonPropertyName("code")] public string Code { get; set; } = string.Empty;

    // Human-readable message
    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;

    // Optional field/path associated with the issue
    [JsonPropertyName("field")] public string? Field { get; set; }

    // Optional hint for resolution
    [JsonPropertyName("hint")] public string? Hint { get; set; }

    // Whether the issue can be auto-recovered with additional input or alt steps
    [JsonPropertyName("recoverable")] public bool? Recoverable { get; set; }
}
