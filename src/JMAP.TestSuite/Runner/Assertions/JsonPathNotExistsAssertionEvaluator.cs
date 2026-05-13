using JMAP.TestSuite.Client;
using JMAP.TestSuite.Scenarios.Models;

namespace JMAP.TestSuite.Runner.Assertions;

public sealed class JsonPathNotExistsAssertionEvaluator : IAssertionEvaluator
{
    public string Type => "jsonPathNotExists";

    public AssertionResult Evaluate(TestAssertionDefinition assertion, JmapHttpResult result)
    {
        var exists = JsonPath.TryResolve(result.ResponseJson, assertion.Path!, out _);

        return new AssertionResult
        {
            Type = Type,
            Outcome = !exists ? TestOutcome.Passed : TestOutcome.Failed,
            Message = !exists ? $"Path '{assertion.Path}' does not exist." : $"Path '{assertion.Path}' exists."
        };
    }
}
