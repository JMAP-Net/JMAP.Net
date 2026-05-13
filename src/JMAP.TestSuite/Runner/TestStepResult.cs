using JMAP.TestSuite.Client;

namespace JMAP.TestSuite.Runner;

public sealed class TestStepResult
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public TestOutcome Outcome { get; set; } = TestOutcome.NotRun;

    public DateTimeOffset StartedAt { get; set; }

    public DateTimeOffset? FinishedAt { get; set; }

    public TimeSpan Duration => FinishedAt is null ? TimeSpan.Zero : FinishedAt.Value - StartedAt;

    public string? ErrorMessage { get; set; }

    public JmapHttpResult? HttpResult { get; set; }

    public List<AssertionResult> Assertions { get; } = [];
}
