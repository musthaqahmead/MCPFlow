using MCPFlow;
using System.Text.Json;

var payload = FlowResult.Payload(FlowBuilder.Orchestrated(PlanStatus.Ok)
        .WithInstruction("Scaffold Payroll screen and components.")
        .WithFollowUp("Wire the new components to the API now?")
        .WithSafety(writesCode: true, touchesFiles: new[] { "src/modules/payroll/**" })
        .AddNextAction("create_screen", new
        {
            screen = "Payroll",
            components = new[] { "OvertimeStartDaySelector", "PayPeriodConfigurator", "PTCHoursOverride" }
        })
        .Build());

Console.WriteLine(JsonSerializer.Serialize(payload));