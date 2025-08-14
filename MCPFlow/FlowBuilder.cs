using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MCPFlow;
public class FlowBuilder
{
    private OrchestratedPlan _plan;

    private static JsonSerializerOptions BaseJson(bool indented = false) => new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = null,
        WriteIndented = indented
    };

    // --- FACTORIES -------------------------------------------------------
    public static FlowBuilder New(FlowKind kind = FlowKind.OrchestratedPlan, PlanStatus status = PlanStatus.Ok, string version = "1")
        => new(kind, status, version);

    public static FlowBuilder Orchestrated(PlanStatus status = PlanStatus.Ok, string version = "1")
        => new(FlowKind.OrchestratedPlan, status, version);

    private FlowBuilder(FlowKind kind, PlanStatus status, string version)
    {
        _plan = new OrchestratedPlan
        {
            Kind = kind == FlowKind.OrchestratedPlan ? "orchestrated_plan" : "orchestrated_plan",
            Version = version,
            Status = status,
            Instruction = "",
            NextActions = new(),
            Errors = null,
            Warnings = null,
            Missing = null,
            PartialResults = null,
            Ask = null,
            RecoveryActions = null,
            Safety = null,
            AffectedPaths = null,
            CorrelationId = null
        };
    }

    // --- CORE ------------------------------------------------------------
    public FlowBuilder WithStatus(PlanStatus status) { _plan.Status = status; return this; }
    public FlowBuilder WithVersion(string version) { _plan.Version = version; return this; }
    public FlowBuilder WithInstruction(string text) { _plan.Instruction = text ?? ""; return this; }
    public FlowBuilder WithFollowUp(string? text) { _plan.FollowUp = string.IsNullOrWhiteSpace(text) ? null : text; return this; }
    public FlowBuilder WithCorrelationId(string? id) { _plan.CorrelationId = string.IsNullOrWhiteSpace(id) ? null : id; return this; }

    public FlowBuilder WithSafety(bool? writesCode = null, IEnumerable<string>? touchesFiles = null)
    {
        _plan.Safety ??= new SafetyInfo();
        if (writesCode.HasValue) _plan.Safety.WritesCode = writesCode;
        if (touchesFiles != null) _plan.Safety.TouchesFiles = touchesFiles.ToList();
        return this;
    }

    public FlowBuilder AddAffectedPaths(params string[] paths)
    {
        _plan.AffectedPaths ??= new List<string>();
        _plan.AffectedPaths.AddRange(paths.Where(p => !string.IsNullOrWhiteSpace(p)));
        return this;
    }

    // --- ACTIONS ---------------------------------------------------------
    public FlowBuilder AddNextAction(string tool, object? args = null, string? label = null, bool? confirm = null)
        => AddActionCore(ActionTarget.Next, tool, args, label, confirm);

    public FlowBuilder AddRecoveryAction(string tool, object? args = null, string? label = null, bool? confirm = null)
        => AddActionCore(ActionTarget.Recovery, tool, args, label, confirm);

    private enum ActionTarget { Next, Recovery }

    private FlowBuilder AddActionCore(ActionTarget target, string tool, object? args, string? label, bool? confirm)
    {
        var call = new ActionCall
        {
            Tool = tool,
            Args = ToJsonObject(args),
            Label = label,
            Confirm = confirm
        };

        if (target == ActionTarget.Next)
        {
            _plan.NextActions ??= new List<ActionCall>();
            _plan.NextActions.Add(call);
        }
        else
        {
            _plan.RecoveryActions ??= new List<ActionCall>();
            _plan.RecoveryActions.Add(call);
        }
        return this;
    }

    // --- ASK (interactive unblock) --------------------------------------
    public FlowBuilder WithAsk(string prompt, IEnumerable<AskField>? fields = null)
    {
        _plan.Ask = new AskArgs { Prompt = prompt, Fields = fields?.ToList() ?? new List<AskField>() };
        return this;
    }

    public FlowBuilder AddAskField(string name, AskFieldType type = AskFieldType.String, bool required = false, IEnumerable<string>? options = null)
    {
        _plan.Ask ??= new AskArgs { Prompt = "Please provide:", Fields = new List<AskField>() };
        _plan.Ask.Fields ??= new List<AskField>();
        _plan.Ask.Fields.Add(new AskField { Name = name, Type = type, Required = required, Options = options?.ToList() });
        return this;
    }

    // --- DIAGNOSTICS -----------------------------------------------------
    public FlowBuilder AddError(string code, string message, string? field = null, string? hint = null, bool? recoverable = null)
    {
        _plan.Errors ??= new List<Issue>();
        _plan.Errors.Add(new Issue { Code = code, Message = message, Field = field, Hint = hint, Recoverable = recoverable });
        return this;
    }

    public FlowBuilder AddWarning(string code, string message, string? field = null, string? hint = null)
    {
        _plan.Warnings ??= new List<Issue>();
        _plan.Warnings.Add(new Issue { Code = code, Message = message, Field = field, Hint = hint, Recoverable = null });
        return this;
    }

    public FlowBuilder AddMissing(params string[] names)
    {
        _plan.Missing ??= new List<string>();
        _plan.Missing.AddRange(names.Where(s => !string.IsNullOrWhiteSpace(s)));
        return this;
    }

    public FlowBuilder AddPartialResult(string step, PartialOutcome outcome = PartialOutcome.Ok, string? detail = null)
    {
        _plan.PartialResults ??= new List<PartialResult>();
        _plan.PartialResults.Add(new PartialResult { Step = step, Outcome = outcome, Detail = detail });
        return this;
    }

    // --- SHORTCUTS -------------------------------------------------------
    public FlowBuilder NotFound(string field, string value, string? hint = null)
        => AddError("NOT_FOUND", $"No match for '{value}'.", field, hint, recoverable: true);

    public FlowBuilder Invalid(string field, string message, string? hint = null)
        => AddError("INVALID", message, field, hint, recoverable: true);

    public FlowBuilder PermissionBlocked(string message = "Insufficient permissions.", string? hint = null)
    { AddError("PERMISSION", message, null, hint, recoverable: false); _plan.Status = PlanStatus.Blocked; return this; }

    public FlowBuilder With(Action<FlowBuilder> apply) { apply(this); return this; }

    // --- OUTPUT ----------------------------------------------------------
    public OrchestratedPlan Build() => _plan;

    public string BuildJson(bool indented = false) => JsonSerializer.Serialize(_plan, BaseJson(indented));

    public JsonElement BuildPayload(bool indented = false) => JsonSerializer.SerializeToElement(_plan, BaseJson(indented));

    // --- INTERNAL --------------------------------------------------------
    private static JsonObject? ToJsonObject(object? args)
    {
        if (args is null) return null;
        if (args is JsonObject jo) return jo;
        var node = JsonSerializer.SerializeToNode(args, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = null });
        return node as JsonObject ?? new JsonObject();
    }
}