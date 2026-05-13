namespace JMAP.TestSuite.Scenarios.Models;

public sealed class TestScenarioDefinition
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    public string Category { get; init; } = "compliance";

    public SpecReference? Spec { get; init; }

    public IReadOnlyList<string> RequiresCapabilities { get; init; } = [];

    public SkipDefinition? Skip { get; init; }

    public IReadOnlyList<TestStepDefinition> Steps { get; init; } = [];
}
