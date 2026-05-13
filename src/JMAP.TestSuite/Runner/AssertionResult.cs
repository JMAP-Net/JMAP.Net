namespace JMAP.TestSuite.Runner;

public sealed class AssertionResult
{
    public required string Type { get; init; }

    public required TestOutcome Outcome { get; init; }

    public required string Message { get; init; }
}
