using System.Runtime.Serialization;

namespace MCPFlow;
public enum PlanStatus
{
    [EnumMember(Value = "ok")] Ok,
    [EnumMember(Value = "error")] Error,
    [EnumMember(Value = "invalid")] Invalid,
    [EnumMember(Value = "blocked")] Blocked,
    [EnumMember(Value = "partial")] Partial
}