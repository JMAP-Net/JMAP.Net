using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Response for CalendarEvent/parse method.
/// As per JMAP Calendars RFC, Section 5.13.
/// </summary>
public sealed class CalendarEventParseResponse
{
    /// <summary>
    /// The id of the account used.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// A map of blob id to parsed CalendarEvent objects.
    /// </summary>
    [JsonPropertyName("parsed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, List<JMAP.Net.Capabilities.Calendars.Types.CalendarEvent>>? Parsed { get; init; }

    /// <summary>
    /// The blob ids that could not be found.
    /// </summary>
    [JsonPropertyName("notFound")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<JmapId>? NotFound { get; init; }

    /// <summary>
    /// The blob ids that were found but could not be parsed as iCalendar.
    /// </summary>
    [JsonPropertyName("notParsable")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<JmapId>? NotParsable { get; init; }
}
