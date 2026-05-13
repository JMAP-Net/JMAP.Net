using System.Text.Json.Nodes;
using JMAP.Net.Hosting;
using JMAP.Net.Hosting.Services;
using JMAP.Net.TestServer.Infrastructure;

namespace JMAP.Net.TestServer.Handlers;

public sealed class CalendarChangesHandler(TestServerData data) : IJmapMethodHandler
{
    public string MethodName => "Calendar/changes";

    public ValueTask<JmapMethodResult> HandleAsync(
        JmapMethodContext context,
        CancellationToken cancellationToken = default)
    {
        var arguments = JsonHandlerSupport.Arguments(context);
        var accountId = JsonHandlerSupport.AccountId(arguments, TestServerData.AccountId);
        var sinceState = arguments["sinceState"]?.GetValue<string>();
        var hasChanges = sinceState == data.CalendarState0;

        return ValueTask.FromResult(new JmapMethodResult
        {
            Name = MethodName,
            Arguments = JsonHandlerSupport.ToArguments(new JsonObject
            {
                ["accountId"] = accountId,
                ["oldState"] = sinceState ?? data.CalendarState,
                ["newState"] = data.CalendarState,
                ["hasMoreChanges"] = false,
                ["created"] = hasChanges ? new JsonArray(TestServerData.CalendarId) : new JsonArray(),
                ["updated"] = new JsonArray(),
                ["destroyed"] = new JsonArray()
            })
        });
    }
}
