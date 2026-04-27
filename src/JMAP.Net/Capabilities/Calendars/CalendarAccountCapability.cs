using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Calendars;

/// <summary>
/// Represents the JMAP Calendars account capability.
/// As per JMAP Calendars RFC, Section 1.5.1.
/// </summary>
public sealed class CalendarAccountCapability
{
    /// <summary>
    /// The capability URI for the JMAP Calendars capability.
    /// </summary>
    public const string CapabilityUri = CalendarCapability.CapabilityUri;

    /// <summary>
    /// The maximum number of Calendars that can be assigned to a single CalendarEvent.
    /// Must be greater than or equal to 1, or null for no limit.
    /// </summary>
    [JsonPropertyName("maxCalendarsPerEvent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUnsignedInt? MaxCalendarsPerEvent { get; init; }

    /// <summary>
    /// The earliest date-time the server is willing to accept for any date stored in a CalendarEvent.
    /// </summary>
    [JsonPropertyName("minDateTime")]
    public required string MinDateTime { get; init; }

    /// <summary>
    /// The latest date-time the server is willing to accept for any date stored in a CalendarEvent.
    /// </summary>
    [JsonPropertyName("maxDateTime")]
    public required string MaxDateTime { get; init; }

    /// <summary>
    /// The maximum duration the user may query over when asking the server to expand recurrences.
    /// ISO 8601 duration format.
    /// </summary>
    [JsonPropertyName("maxExpandedQueryDuration")]
    public required string MaxExpandedQueryDuration { get; init; }

    /// <summary>
    /// The maximum number of participants a single event may have, or null for no limit.
    /// </summary>
    [JsonPropertyName("maxParticipantsPerEvent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUnsignedInt? MaxParticipantsPerEvent { get; init; }

    /// <summary>
    /// If true, the user may create a calendar in this account.
    /// </summary>
    [JsonPropertyName("mayCreateCalendar")]
    public required bool MayCreateCalendar { get; init; }
}
