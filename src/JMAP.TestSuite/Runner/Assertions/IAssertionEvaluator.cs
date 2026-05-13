using JMAP.TestSuite.Client;
using JMAP.TestSuite.Scenarios.Models;

namespace JMAP.TestSuite.Runner.Assertions;

public interface IAssertionEvaluator
{
    string Type { get; }

    AssertionResult Evaluate(TestAssertionDefinition assertion, JmapHttpResult result);
}
