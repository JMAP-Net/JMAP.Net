using JMAP.TestSuite.Client;
using JMAP.TestSuite.Scenarios.Models;

namespace JMAP.TestSuite.Runner.Assertions;

public sealed class JsonPathExistsAssertionEvaluator : IAssertionEvaluator
{
    public string Type => "jsonPathExists";

    public AssertionResult Evaluate(TestAssertionDefinition assertion, JmapHttpResult result)
    {
        var passed = JsonPath.TryResolve(result.ResponseJson, assertion.Path!, out _);

        return new AssertionResult
        {
            Type = Type,
            Outcome = passed ? TestOutcome.Passed : TestOutcome.Failed,
            Message = passed ? $"Path '{assertion.Path}' exists." : $"Path '{assertion.Path}' was not found."
        };
    }
}
