using System.Text.Json;
using System.Text.Json.Serialization;

namespace MCPFlow;
public static class FlowResult
{
    private static JsonSerializerOptions Base(bool indented = false) => new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = null,
        WriteIndented = indented,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public static JsonElement Payload(OrchestratedPlan plan, bool indented = false)
        => JsonSerializer.SerializeToElement(plan, Base(indented));

    public static JsonElement Payload(FlowBuilder builder, bool indented = false)
        => Payload(builder.Build(), indented);

    public static string Json(OrchestratedPlan plan, bool indented = false)
        => JsonSerializer.Serialize(plan, Base(indented));

    public static string Json(FlowBuilder builder, bool indented = false)
        => Json(builder.Build(), indented);
}