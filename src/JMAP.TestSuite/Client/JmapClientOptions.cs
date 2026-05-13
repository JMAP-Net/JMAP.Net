namespace JMAP.TestSuite.Client;

public sealed class JmapClientOptions
{
    public required Uri ApiUrl { get; init; }

    public Uri? SessionUrl { get; init; }

    public required string BearerToken { get; init; }

    public IReadOnlyList<string> DefaultUsing { get; init; } = [];

    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
}
