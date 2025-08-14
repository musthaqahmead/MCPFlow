using System.Text.Json.Serialization;

namespace MCPFlow;
public class AskArgs
{
    [JsonPropertyName("prompt")] public string Prompt { get; set; } = string.Empty;

    // Fields to collect; for type == select, provide Options.
    [JsonPropertyName("fields")] public List<AskField>? Fields { get; set; }
}
