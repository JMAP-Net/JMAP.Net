using System.Text.Json.Nodes;
using JMAP.TestSuite.Client;
using JMAP.TestSuite.Scenarios.Models;

namespace JMAP.TestSuite.Runner.Assertions;

public sealed class JsonPathArrayCountAssertionEvaluator : IAssertionEvaluator
{
    public string Type => "jsonPathArrayCount";

    public AssertionResult Evaluate(TestAssertionDefinition assertion, JmapHttpResult result)
    {
        var found = JsonPath.TryResolve(result.ResponseJson, assertion.Path!, out var actual);
        var count = actual is JsonArray array ? array.Count : (int?)null;
        var passed = found && count == assertion.Count;

        return new AssertionResult
        {
            Type = Type,
            Outcome = passed ? TestOutcome.Passed : TestOutcome.Failed,
            Message = passed
                ? $"Path '{assertion.Path}' contains {assertion.Count} item(s)."
                : $"Expected path '{assertion.Path}' to contain {assertion.Count} item(s), got {(count?.ToString() ?? "<not an array>")}."
        };
    }
}
