namespace JMAP.TestSuite.Runner;

public sealed class TestRunResult
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public string? SpecDocument { get; init; }

    public string? SpecSection { get; init; }

    public string? SpecRequirement { get; init; }

    public TestOutcome Outcome { get; set; } = TestOutcome.NotRun;

    public DateTimeOffset StartedAt { get; set; }

    public DateTimeOffset? FinishedAt { get; set; }

    public TimeSpan Duration => FinishedAt is null ? TimeSpan.Zero : FinishedAt.Value - StartedAt;

    public string? ErrorMessage { get; set; }

    public List<TestGroupResult> Groups { get; } = [];
}
