using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Calendars.Methods.CalendarEvent;

/// <summary>
/// Filter condition for CalendarEvent/query.
/// As per JMAP Calendars RFC, Section 5.11.1.
/// </summary>
public class CalendarEventFilterCondition
{
    /// <summary>
    /// A list of calendar ids. An event must be in ANY of these calendars to match.
    /// </summary>
    [JsonPropertyName("inCalendars")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<JmapId>? InCalendars { get; init; }

    /// <summary>
    /// The end of the event (or any recurrence) in the given time zone must be after this date.
    /// </summary>
    [JsonPropertyName("after")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? After { get; init; }  // LocalDate format

    /// <summary>
    /// The start of the event (or any recurrence) in the given time zone must be before this date.
    /// </summary>
    [JsonPropertyName("before")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Before { get; init; }  // LocalDate format

    /// <summary>
    /// Looks for text in title, description, locations, participants, and other textual properties.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; init; }

    /// <summary>
    /// Looks for text in the title property.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; init; }

    /// <summary>
    /// Looks for text in the description property.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    /// <summary>
    /// Looks for text in the locations property (matching name/description).
    /// </summary>
    [JsonPropertyName("location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Location { get; init; }

    /// <summary>
    /// Looks for text in participants with the "owner" role.
    /// </summary>
    [JsonPropertyName("owner")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Owner { get; init; }

    /// <summary>
    /// Looks for text in participants with the "attendee" role.
    /// </summary>
    [JsonPropertyName("attendee")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Attendee { get; init; }

    /// <summary>
    /// The participation status to match.
    /// </summary>
    [JsonPropertyName("participationStatus")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ParticipationStatus { get; init; }

    /// <summary>
    /// The uid of the event must exactly match this string.
    /// </summary>
    [JsonPropertyName("uid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Uid { get; init; }
}
