namespace JMAP.TestSuite.Scenarios.Models;

public sealed class TestGroupDefinition
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    public SpecReference? Spec { get; init; }

    public IReadOnlyList<string> RequiresCapabilities { get; init; } = [];

    public SkipDefinition? Skip { get; init; }

    public IReadOnlyList<TestScenarioDefinition> Scenarios { get; init; } = [];
}
