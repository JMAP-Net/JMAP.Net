using System.Globalization;
using System.Text.Json.Nodes;
using JMAP.TestSuite.Scenarios.Models;
using YamlDotNet.RepresentationModel;

namespace JMAP.TestSuite.Scenarios.Loading;

public sealed class YamlTestSuiteLoader : ITestSuiteLoader
{
    public async Task<TestSuiteDefinition> LoadAsync(string path, CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(path);
        using var reader = new StreamReader(stream);
        var yaml = new YamlStream();
        yaml.Load(reader);

        if (yaml.Documents.Count == 0 || yaml.Documents[0].RootNode is not YamlMappingNode root)
        {
            throw new InvalidDataException("The YAML file must contain a suite object.");
        }

        return ParseSuite(root);
    }

    private static TestSuiteDefinition ParseSuite(YamlMappingNode root)
    {
        return new TestSuiteDefinition
        {
            Id = RequiredScalar(root, "id"),
            Name = RequiredScalar(root, "name"),
            Description = OptionalScalar(root, "description"),
            Category = OptionalScalar(root, "category") ?? "compliance",
            Spec = ParseSpec(OptionalMap(root, "spec")),
            RequiresCapabilities = OptionalStringSequence(root, "requiresCapabilities").ToList(),
            Skip = ParseSkip(OptionalMap(root, "skip")),
            Defaults = ParseDefaults(OptionalMap(root, "defaults")),
            Groups = OptionalSequence(root, "groups").Select(ParseGroup).ToList()
        };
    }

    private static SpecReference? ParseSpec(YamlMappingNode? node)
    {
        if (node is null)
        {
            return null;
        }

        return new SpecReference
        {
            Document = RequiredScalar(node, "document"),
            Section = OptionalScalar(node, "section"),
            Requirement = OptionalScalar(node, "requirement")
        };
    }

    private static SkipDefinition? ParseSkip(YamlMappingNode? node)
    {
        if (node is null)
        {
            return null;
        }

        return new SkipDefinition
        {
            Reason = RequiredScalar(node, "reason")
        };
    }

    private static TestSuiteDefaults ParseDefaults(YamlMappingNode? node)
    {
        return new TestSuiteDefaults
        {
            Using = node is null ? [] : OptionalStringSequence(node, "using").ToList()
        };
    }

    private static TestGroupDefinition ParseGroup(YamlNode node)
    {
        var map = AsMap(node, "group");
        return new TestGroupDefinition
        {
            Id = RequiredScalar(map, "id"),
            Name = RequiredScalar(map, "name"),
            Description = OptionalScalar(map, "description"),
            Spec = ParseSpec(OptionalMap(map, "spec")),
            RequiresCapabilities = OptionalStringSequence(map, "requiresCapabilities").ToList(),
            Skip = ParseSkip(OptionalMap(map, "skip")),
            Scenarios = OptionalSequence(map, "scenarios").Select(ParseScenario).ToList()
        };
    }

    private static TestScenarioDefinition ParseScenario(YamlNode node)
    {
        var map = AsMap(node, "scenario");
        return new TestScenarioDefinition
        {
            Id = RequiredScalar(map, "id"),
            Name = RequiredScalar(map, "name"),
            Description = OptionalScalar(map, "description"),
            Category = OptionalScalar(map, "category") ?? "compliance",
            Spec = ParseSpec(OptionalMap(map, "spec")),
            RequiresCapabilities = OptionalStringSequence(map, "requiresCapabilities").ToList(),
            Skip = ParseSkip(OptionalMap(map, "skip")),
            Steps = OptionalSequence(map, "steps").Select(ParseStep).ToList()
        };
    }

    private static TestStepDefinition ParseStep(YamlNode node)
    {
        var map = AsMap(node, "step");
        return new TestStepDefinition
        {
            Id = RequiredScalar(map, "id"),
            Type = RequiredScalar(map, "type"),
            Request = OptionalNode(map, "request") is { } request ? ToJsonNode(request)?.AsObject() : null,
            Body = OptionalScalar(map, "body"),
            Expect = OptionalSequence(map, "expect").Select(ParseAssertion).ToList()
        };
    }

    private static TestAssertionDefinition ParseAssertion(YamlNode node)
    {
        var map = AsMap(node, "assertion");
        if (map.Children.Count != 1)
        {
            throw new InvalidDataException("Each assertion must contain exactly one assertion type.");
        }

        var pair = map.Children.Single();
        var type = ((YamlScalarNode)pair.Key).Value ?? "";
        var valueNode = pair.Value;

        return type switch
        {
            "statusCode" => new TestAssertionDefinition
            {
                Type = type,
                StatusCode = int.Parse(ScalarValue(valueNode), CultureInfo.InvariantCulture)
            },
            "jsonPathExists" or "jsonPathNotExists" => new TestAssertionDefinition
            {
                Type = type,
                Path = ScalarValue(valueNode)
            },
            "jsonPathEquals" => ParseJsonPathEquals(type, AsMap(valueNode, type)),
            "jsonPathArrayCount" => ParseJsonPathArrayCount(type, AsMap(valueNode, type)),
            "methodResponse" => ParseMethodResponse(type, AsMap(valueNode, type)),
            _ => new TestAssertionDefinition { Type = type }
        };
    }

    private static TestAssertionDefinition ParseJsonPathEquals(string type, YamlMappingNode map)
        => new()
        {
            Type = type,
            Path = RequiredScalar(map, "path"),
            Value = ToJsonNode(RequiredNode(map, "value"))
        };

    private static TestAssertionDefinition ParseJsonPathArrayCount(string type, YamlMappingNode map)
        => new()
        {
            Type = type,
            Path = RequiredScalar(map, "path"),
            Count = int.Parse(RequiredScalar(map, "count"), CultureInfo.InvariantCulture)
        };

    private static TestAssertionDefinition ParseMethodResponse(string type, YamlMappingNode map)
        => new()
        {
            Type = type,
            CallId = RequiredScalar(map, "callId"),
            Name = RequiredScalar(map, "name")
        };

    private static JsonNode? ToJsonNode(YamlNode node)
    {
        return node switch
        {
            YamlScalarNode scalar => ToJsonScalar(scalar),
            YamlSequenceNode sequence => new JsonArray(sequence.Children.Select(ToJsonNode).ToArray()),
            YamlMappingNode map => ToJsonObject(map),
            _ => null
        };
    }

    private static JsonNode? ToJsonScalar(YamlScalarNode scalar)
    {
        var value = scalar.Value;
        if (value is null || scalar.Tag == "tag:yaml.org,2002:null")
        {
            return null;
        }

        if (bool.TryParse(value, out var boolValue))
        {
            return JsonValue.Create(boolValue);
        }

        if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
        {
            return JsonValue.Create(longValue);
        }

        if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleValue))
        {
            return JsonValue.Create(doubleValue);
        }

        return JsonValue.Create(value);
    }

    private static JsonObject ToJsonObject(YamlMappingNode map)
    {
        var obj = new JsonObject();
        foreach (var pair in map.Children)
        {
            obj[ScalarValue(pair.Key)] = ToJsonNode(pair.Value);
        }

        return obj;
    }

    private static YamlMappingNode AsMap(YamlNode node, string name)
        => node as YamlMappingNode ?? throw new InvalidDataException($"The {name} entry must be an object.");

    private static string RequiredScalar(YamlMappingNode node, string key)
        => ScalarValue(RequiredNode(node, key));

    private static string? OptionalScalar(YamlMappingNode node, string key)
        => OptionalNode(node, key) is { } value ? ScalarValue(value) : null;

    private static YamlMappingNode? OptionalMap(YamlMappingNode node, string key)
        => OptionalNode(node, key) as YamlMappingNode;

    private static IEnumerable<YamlNode> OptionalSequence(YamlMappingNode node, string key)
        => OptionalNode(node, key) is YamlSequenceNode sequence ? sequence.Children : [];

    private static IEnumerable<string> OptionalStringSequence(YamlMappingNode node, string key)
        => OptionalSequence(node, key).Select(ScalarValue);

    private static YamlNode RequiredNode(YamlMappingNode node, string key)
        => OptionalNode(node, key) ?? throw new InvalidDataException($"Missing required field '{key}'.");

    private static YamlNode? OptionalNode(YamlMappingNode node, string key)
    {
        var keyNode = new YamlScalarNode(key);
        return node.Children.TryGetValue(keyNode, out var value) ? value : null;
    }

    private static string ScalarValue(YamlNode node)
        => node is YamlScalarNode scalar
            ? scalar.Value ?? ""
            : throw new InvalidDataException("Expected a scalar YAML value.");
}
