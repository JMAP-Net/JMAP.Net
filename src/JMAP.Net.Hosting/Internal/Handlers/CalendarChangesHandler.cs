using JMAP.Net.Capabilities.Calendars.Methods.Calendar;
using JMAP.Net.Hosting.Engine;
using JMAP.Net.Hosting.Engine.Calendar;
using JMAP.Net.Hosting.Services;

namespace JMAP.Net.Hosting.Internal.Handlers;

internal sealed class CalendarChangesHandler(
    IJmapUserContextProvider userContextProvider,
    ICalendarEngine calendarEngine)
    : IJmapMethodHandler
{
    public string MethodName => "Calendar/changes";

    public async ValueTask<JmapMethodResult> HandleAsync(
        JmapMethodContext context,
        CancellationToken cancellationToken = default)
    {
        var request = JmapMethodHandlerJson.DeserializeArguments<CalendarChangesRequest>(context.Invocation.Arguments);
        var executionContext = await userContextProvider.GetContextAsync(context.Transaction, cancellationToken);
        var response = await calendarEngine.ChangesAsync(executionContext, request, cancellationToken);

        return new JmapMethodResult
        {
            Name = MethodName,
            Arguments = JmapMethodHandlerJson.ToArguments(response)
        };
    }
}
