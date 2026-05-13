using JMAP.Net.Capabilities.Calendars.Methods.Calendar;

namespace JMAP.Net.Hosting.Engine.Calendar;

/// <summary>
/// Executes JMAP Calendar method semantics.
/// </summary>
public interface ICalendarEngine
{
    /// <summary>
    /// Executes Calendar/get.
    /// </summary>
    /// <param name="context">The JMAP execution context.</param>
    /// <param name="request">The Calendar/get request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The Calendar/get response.</returns>
    ValueTask<CalendarGetResponse> GetAsync(
        JmapExecutionContext context,
        CalendarGetRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes Calendar/query.
    /// </summary>
    /// <param name="context">The JMAP execution context.</param>
    /// <param name="request">The Calendar/query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The Calendar/query response.</returns>
    ValueTask<CalendarQueryResponse> QueryAsync(
        JmapExecutionContext context,
        CalendarQueryRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes Calendar/changes.
    /// </summary>
    /// <param name="context">The JMAP execution context.</param>
    /// <param name="request">The Calendar/changes request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The Calendar/changes response.</returns>
    ValueTask<CalendarChangesResponse> ChangesAsync(
        JmapExecutionContext context,
        CalendarChangesRequest request,
        CancellationToken cancellationToken = default);
}
