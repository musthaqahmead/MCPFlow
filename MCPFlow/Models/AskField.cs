using System.Text.Json.Serialization;

namespace MCPFlow;
public class AskField
{
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")] public AskFieldType Type { get; set; } = AskFieldType.String;

    [JsonPropertyName("required")] public bool? Required { get; set; }

    [JsonPropertyName("options")] public List<string>? Options { get; set; }
}
