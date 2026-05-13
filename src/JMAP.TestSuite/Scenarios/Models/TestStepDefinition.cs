using System.Text.Json.Nodes;

namespace JMAP.TestSuite.Scenarios.Models;

public sealed class TestStepDefinition
{
    public required string Id { get; init; }

    public required string Type { get; init; }

    public JsonObject? Request { get; init; }

    public string? Body { get; init; }

    public IReadOnlyList<TestAssertionDefinition> Expect { get; init; } = [];
}
