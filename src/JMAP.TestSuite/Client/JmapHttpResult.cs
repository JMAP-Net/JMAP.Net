using System.Text.Json.Nodes;

namespace JMAP.TestSuite.Client;

public sealed class JmapHttpResult
{
    public required HttpMethod Method { get; init; }

    public required Uri Url { get; init; }

    public required int StatusCode { get; init; }

    public required string? RequestBody { get; init; }

    public required string ResponseBody { get; init; }

    public JsonNode? ResponseJson { get; init; }

    public string? JsonParseError { get; init; }

    public required TimeSpan Duration { get; init; }

    public IReadOnlyDictionary<string, IReadOnlyList<string>> ResponseHeaders { get; init; }
        = new Dictionary<string, IReadOnlyList<string>>();
}
