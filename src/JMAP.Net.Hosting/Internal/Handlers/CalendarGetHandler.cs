using System.Text.Json;
using System.Text.Json.Nodes;
using JMAP.Net.Capabilities.Calendars.Methods.Calendar;
using JMAP.Net.Hosting.Engine;
using JMAP.Net.Hosting.Engine.Calendar;
using JMAP.Net.Hosting.Services;

namespace JMAP.Net.Hosting.Internal.Handlers;

internal sealed class CalendarGetHandler(
    IJmapUserContextProvider userContextProvider,
    ICalendarEngine calendarEngine)
    : IJmapMethodHandler
{
    public string MethodName => "Calendar/get";

    public async ValueTask<JmapMethodResult> HandleAsync(
        JmapMethodContext context,
        CancellationToken cancellationToken = default)
    {
        var request = JmapMethodHandlerJson.DeserializeArguments<CalendarGetRequest>(context.Invocation.Arguments);
        var executionContext = await userContextProvider.GetContextAsync(context.Transaction, cancellationToken);
        var response = await calendarEngine.GetAsync(executionContext, request, cancellationToken);

        var arguments = JmapMethodHandlerJson.ToArguments(response);
        if (request.Properties is not null)
        {
            arguments = ProjectListProperties(arguments, request.Properties);
        }

        return new JmapMethodResult
        {
            Name = MethodName,
            Arguments = arguments
        };
    }

    private static Dictionary<string, object?> ProjectListProperties(
        Dictionary<string, object?> arguments,
        IReadOnlyCollection<string> properties)
    {
        var requested = properties.ToHashSet(StringComparer.Ordinal);
        requested.Add("id");

        var root = JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize(arguments))
            ?? throw new JsonException("Could not serialize Calendar/get arguments as JSON.");

        if (root["list"] is not JsonArray list)
        {
            return arguments;
        }

        var projectedList = new JsonArray();
        foreach (var item in list.OfType<JsonObject>())
        {
            var projected = new JsonObject();
            foreach (var property in requested)
            {
                if (item.TryGetPropertyValue(property, out var value))
                {
                    projected[property] = value?.DeepClone();
                }
            }

            projectedList.Add(projected);
        }

        root["list"] = projectedList;

        return JsonSerializer.Deserialize<Dictionary<string, object?>>(root.ToJsonString())
            ?? throw new JsonException("Could not serialize projected Calendar/get arguments.");
    }
}
