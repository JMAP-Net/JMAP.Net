using JMAP.TestSuite.Scenarios.Models;

namespace JMAP.TestSuite.Scenarios.Loading;

public interface ITestSuiteLoader
{
    Task<TestSuiteDefinition> LoadAsync(string path, CancellationToken cancellationToken);
}
