using System.Text.Json.Nodes;
using JMAP.TestSuite.Client;
using JMAP.TestSuite.Scenarios.Models;

namespace JMAP.TestSuite.Runner.Assertions;

public sealed class JsonPathEqualsAssertionEvaluator : IAssertionEvaluator
{
    public string Type => "jsonPathEquals";

    public AssertionResult Evaluate(TestAssertionDefinition assertion, JmapHttpResult result)
    {
        var found = JsonPath.TryResolve(result.ResponseJson, assertion.Path!, out var actual);
        var passed = found && JsonNode.DeepEquals(actual, assertion.Value);

        return new AssertionResult
        {
            Type = Type,
            Outcome = passed ? TestOutcome.Passed : TestOutcome.Failed,
            Message = passed
                ? $"Path '{assertion.Path}' equals expected value."
                : $"Expected path '{assertion.Path}' to equal {assertion.Value?.ToJsonString() ?? "null"}, got {(found ? actual?.ToJsonString() ?? "null" : "<missing>")}."
        };
    }
}
