using System.Text.Json;
using System.Text.Json.Nodes;
using JMAP.Net.Hosting;

namespace JMAP.Net.TestServer.Handlers;

internal static class JsonHandlerSupport
{
    public static Dictionary<string, object?> ToArguments(JsonObject value)
    {
        return JsonSerializer.Deserialize<Dictionary<string, object?>>(value.ToJsonString())
               ?? new Dictionary<string, object?>();
    }

    public static JsonObject Arguments(JmapMethodContext context)
    {
        return JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize(context.Invocation.Arguments))
               ?? new JsonObject();
    }

    public static string AccountId(JsonObject arguments, string fallback)
        => arguments["accountId"]?.GetValue<string>() ?? fallback;

    public static string[]? Ids(JsonObject arguments)
    {
        return arguments["ids"] is JsonArray ids
            ? ids.Select(x => x?.GetValue<string>()).Where(x => x is not null).Cast<string>().ToArray()
            : null;
    }

    public static string[]? Properties(JsonObject arguments)
    {
        return arguments["properties"] is JsonArray properties
            ? properties.Select(x => x?.GetValue<string>()).Where(x => x is not null).Cast<string>().ToArray()
            : null;
    }

    public static JsonObject Project(JsonObject source, string[]? properties)
    {
        if (properties is null)
        {
            return source.DeepClone().AsObject();
        }

        var projected = new JsonObject();
        foreach (var property in properties)
        {
            if (source.TryGetPropertyValue(property, out var value))
            {
                projected[property] = value?.DeepClone();
            }
        }

        return projected;
    }
}
