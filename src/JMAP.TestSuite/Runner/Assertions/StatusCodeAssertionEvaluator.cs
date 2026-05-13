using JMAP.TestSuite.Client;
using JMAP.TestSuite.Scenarios.Models;

namespace JMAP.TestSuite.Runner.Assertions;

public sealed class StatusCodeAssertionEvaluator : IAssertionEvaluator
{
    public string Type => "statusCode";

    public AssertionResult Evaluate(TestAssertionDefinition assertion, JmapHttpResult result)
    {
        var expected = assertion.StatusCode;
        var passed = result.StatusCode == expected;

        return new AssertionResult
        {
            Type = Type,
            Outcome = passed ? TestOutcome.Passed : TestOutcome.Failed,
            Message = passed
                ? $"HTTP status is {expected}."
                : $"Expected HTTP status {expected}, got {result.StatusCode}."
        };
    }
}
