namespace JMAP.TestSuite.Scenarios.Models;

public sealed class TestSuiteDefinition
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    public string Category { get; init; } = "compliance";

    public SpecReference? Spec { get; init; }

    public IReadOnlyList<string> RequiresCapabilities { get; init; } = [];

    public SkipDefinition? Skip { get; init; }

    public TestSuiteDefaults Defaults { get; init; } = new();

    public IReadOnlyList<TestGroupDefinition> Groups { get; init; } = [];
}

public sealed class TestSuiteDefaults
{
    public IReadOnlyList<string> Using { get; init; } = [];
}

public sealed class SpecReference
{
    public required string Document { get; init; }

    public string? Section { get; init; }

    public string? Requirement { get; init; }
}

public sealed class SkipDefinition
{
    public required string Reason { get; init; }
}
