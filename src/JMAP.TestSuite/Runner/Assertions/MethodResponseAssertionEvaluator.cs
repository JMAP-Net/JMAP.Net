using System.Text.Json.Nodes;
using JMAP.TestSuite.Client;
using JMAP.TestSuite.Scenarios.Models;

namespace JMAP.TestSuite.Runner.Assertions;

public sealed class MethodResponseAssertionEvaluator : IAssertionEvaluator
{
    public string Type => "methodResponse";

    public AssertionResult Evaluate(TestAssertionDefinition assertion, JmapHttpResult result)
    {
        var found = false;

        if (JsonPath.TryResolve(result.ResponseJson, "$.methodResponses", out var node) && node is JsonArray responses)
        {
            found = responses.OfType<JsonArray>().Any(triple =>
                triple.Count >= 3
                && triple[0]?.GetValue<string>() == assertion.Name
                && triple[2]?.GetValue<string>() == assertion.CallId);
        }

        return new AssertionResult
        {
            Type = Type,
            Outcome = found ? TestOutcome.Passed : TestOutcome.Failed,
            Message = found
                ? $"Found method response '{assertion.Name}' for call id '{assertion.CallId}'."
                : $"Missing method response '{assertion.Name}' for call id '{assertion.CallId}'."
        };
    }
}
