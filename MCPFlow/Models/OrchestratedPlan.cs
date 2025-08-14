using System.Text.Json.Serialization;

namespace MCPFlow;
public class OrchestratedPlan
{
    [JsonPropertyName("kind")] public string Kind { get; set; } = "orchestrated_plan";
    [JsonPropertyName("version")] public string Version { get; set; } = "1";
    [JsonPropertyName("status")] public PlanStatus Status { get; set; } = PlanStatus.Ok;

    // Human-facing summary
    [JsonPropertyName("instruction")] public string Instruction { get; set; } = string.Empty;

    // Main actions to perform next (may be empty on error/blocked)
    [JsonPropertyName("next_actions")] public List<ActionCall> NextActions { get; set; } = new();

    // After actions complete, ask this
    [JsonPropertyName("follow_up")] public string? FollowUp { get; set; }

    // Diagnostics
    [JsonPropertyName("errors")] public List<Issue>? Errors { get; set; }
    [JsonPropertyName("warnings")] public List<Issue>? Warnings { get; set; }
    [JsonPropertyName("missing")] public List<string>? Missing { get; set; }
    [JsonPropertyName("partial_results")] public List<PartialResult>? PartialResults { get; set; }

    // Optional UX to unblock (e.g., prompt user for missing fields)
    [JsonPropertyName("ask")] public AskArgs? Ask { get; set; }

    // Safe alternatives when blocked or recoverable errors occur
    [JsonPropertyName("recovery_actions")] public List<ActionCall>? RecoveryActions { get; set; }

    // Safety / review
    [JsonPropertyName("safety")] public SafetyInfo? Safety { get; set; }
    [JsonPropertyName("affected_paths")] public List<string>? AffectedPaths { get; set; }

    // Tracing
    [JsonPropertyName("correlation_id")] public string? CorrelationId { get; set; }
}
