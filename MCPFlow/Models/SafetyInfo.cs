using System.Text.Json.Serialization;

namespace MCPFlow;
public class SafetyInfo
{
    [JsonPropertyName("writes_code")] public bool? WritesCode { get; set; }

    [JsonPropertyName("touches_files")] public List<string>? TouchesFiles { get; set; }
}
