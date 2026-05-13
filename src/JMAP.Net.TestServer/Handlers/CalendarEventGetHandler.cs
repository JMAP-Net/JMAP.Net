using System.Text.Json.Nodes;
using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Services;
using JMAP.Net.TestServer.Infrastructure;

namespace JMAP.Net.TestServer.Handlers;

public sealed class CalendarEventGetHandler(TestServerData data) : IJmapMethodHandler
{
    public string MethodName => "CalendarEvent/get";

    public ValueTask<JmapMethodResult> HandleAsync(
        JmapMethodContext context,
        CancellationToken cancellationToken = default)
    {
        var arguments = JsonHandlerSupport.Arguments(context);
        var ids = JsonHandlerSupport.Ids(arguments);
        var properties = JsonHandlerSupport.Properties(arguments);
        var accountId = JsonHandlerSupport.AccountId(arguments, TestServerData.AccountId);

        var list = new JsonArray();
        var notFound = new JsonArray();

        if (ids is null || ids.Contains(TestServerData.EventId, StringComparer.Ordinal))
        {
            list.Add(JsonHandlerSupport.Project(data.StandupEvent, properties));
        }

        if (ids is not null)
        {
            foreach (var id in ids.Where(id => id != TestServerData.EventId))
            {
                notFound.Add(id);
            }
        }

        return ValueTask.FromResult(new JmapMethodResult
        {
            Name = MethodName,
            Arguments = JsonHandlerSupport.ToArguments(new JsonObject
            {
                ["accountId"] = accountId,
                ["state"] = data.CalendarEventState,
                ["list"] = list,
                ["notFound"] = notFound
            })
        });
    }
}
