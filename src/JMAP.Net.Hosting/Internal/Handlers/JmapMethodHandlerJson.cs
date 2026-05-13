using System.Text.Json;

namespace JMAP.Net.Hosting.Internal.Handlers;

internal static class JmapMethodHandlerJson
{
    public static T DeserializeArguments<T>(Dictionary<string, object?> arguments)
    {
        var json = JsonSerializer.Serialize(arguments);
        return JsonSerializer.Deserialize<T>(json)
            ?? throw new JsonException($"Could not deserialize arguments as {typeof(T).Name}.");
    }

    public static Dictionary<string, object?> ToArguments<T>(T value)
    {
        var json = JsonSerializer.Serialize(value);
        return JsonSerializer.Deserialize<Dictionary<string, object?>>(json)
            ?? throw new JsonException($"Could not serialize {typeof(T).Name} as JMAP method arguments.");
    }
}
