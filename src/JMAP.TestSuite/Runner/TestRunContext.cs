using JMAP.TestSuite.Client;
using JMAP.TestSuite.Scenarios.Models;

namespace JMAP.TestSuite.Runner;

public sealed class TestRunContext
{
    public required TestSuiteDefinition Suite { get; init; }

    public required JmapClientOptions Options { get; init; }
}
