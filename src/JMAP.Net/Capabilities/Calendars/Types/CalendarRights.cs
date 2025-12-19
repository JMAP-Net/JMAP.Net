using System.Text.Json.Serialization;

namespace JMAP.Net.Capabilities.Calendars.Types;

/// <summary>
/// Represents access rights for a calendar.
/// As per JMAP Calendars RFC, Section 4.
/// </summary>
public class CalendarRights
{
    /// <summary>
    /// The user may read the free-busy information for this calendar.
    /// </summary>
    [JsonPropertyName("mayReadFreeBusy")]
    public required bool MayReadFreeBusy { get; init; }

    /// <summary>
    /// The user may fetch the events in this calendar.
    /// </summary>
    [JsonPropertyName("mayReadItems")]
    public required bool MayReadItems { get; init; }

    /// <summary>
    /// The user may create, modify or destroy all events in this calendar.
    /// If true, mayWriteOwn, mayUpdatePrivate and mayRSVP must all also be true.
    /// </summary>
    [JsonPropertyName("mayWriteAll")]
    public required bool MayWriteAll { get; init; }

    /// <summary>
    /// The user may create, modify or destroy an event if they are the owner
    /// or the event has no owner.
    /// </summary>
    [JsonPropertyName("mayWriteOwn")]
    public required bool MayWriteOwn { get; init; }

    /// <summary>
    /// The user may modify per-user properties on all events in the calendar.
    /// </summary>
    [JsonPropertyName("mayUpdatePrivate")]
    public required bool MayUpdatePrivate { get; init; }

    /// <summary>
    /// The user may modify participation properties of participants corresponding
    /// to their identities.
    /// </summary>
    [JsonPropertyName("mayRSVP")]
    public required bool MayRSVP { get; init; }

    /// <summary>
    /// The user may modify the shareWith property for this calendar.
    /// </summary>
    [JsonPropertyName("mayAdmin")]
    public required bool MayAdmin { get; init; }

    /// <summary>
    /// The user may delete the calendar itself.
    /// </summary>
    [JsonPropertyName("mayDelete")]
    public required bool MayDelete { get; init; }
}
