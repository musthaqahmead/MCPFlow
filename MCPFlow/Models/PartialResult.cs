using System.Text.Json.Serialization;

namespace MCPFlow;
public class PartialResult
{
    [JsonPropertyName("step")] public string Step { get; set; } = string.Empty;

    [JsonPropertyName("outcome")] public PartialOutcome Outcome { get; set; } = PartialOutcome.Ok;

    [JsonPropertyName("detail")] public string? Detail { get; set; }
}
