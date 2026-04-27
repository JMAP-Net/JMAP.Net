using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Request for CalendarEvent/parse method.
/// As per JMAP Calendars RFC, Section 5.13.
/// </summary>
public sealed class CalendarEventParseRequest
{
    /// <summary>
    /// The id of the account to use.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// The ids of the blobs to parse as iCalendar files.
    /// </summary>
    [JsonPropertyName("blobIds")]
    public required List<JmapId> BlobIds { get; init; }
}
