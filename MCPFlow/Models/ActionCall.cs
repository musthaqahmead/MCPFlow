using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MCPFlow;
public class ActionCall
{
    [JsonPropertyName("tool")] public string Tool { get; set; } = string.Empty;

    // Free-form arguments to the tool. Use AskArgs schema when tool == "ask_user".
    // JsonObject keeps structure flexible; construct with JsonNode.Parse or new JsonObject().
    [JsonPropertyName("args")] public JsonObject? Args { get; set; }

    // Optional UI label
    [JsonPropertyName("label")] public string? Label { get; set; }

    // If true, agent should explicitly confirm before executing this step.
    [JsonPropertyName("confirm")] public bool? Confirm { get; set; }
}
