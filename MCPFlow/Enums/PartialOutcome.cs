using System.Text.Json.Serialization;

namespace MCPFlow;
public enum PartialOutcome
{
    [JsonPropertyName("ok")] Ok,
    [JsonPropertyName("skipped")] Skipped,
    [JsonPropertyName("failed")] Failed
}
