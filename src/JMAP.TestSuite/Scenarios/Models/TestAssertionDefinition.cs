using System.Text.Json.Nodes;

namespace JMAP.TestSuite.Scenarios.Models;

public sealed class TestAssertionDefinition
{
    public required string Type { get; init; }

    public string? Path { get; init; }

    public JsonNode? Value { get; init; }

    public int? Count { get; init; }

    public int? StatusCode { get; init; }

    public string? CallId { get; init; }

    public string? Name { get; init; }
}
