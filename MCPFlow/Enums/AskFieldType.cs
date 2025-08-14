using System.Text.Json.Serialization;

namespace MCPFlow;
public enum AskFieldType
{
    [JsonPropertyName("string")] String,
    [JsonPropertyName("number")] Number,
    [JsonPropertyName("select")] Select
}
