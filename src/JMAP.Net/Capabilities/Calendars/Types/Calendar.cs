using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JSCalendar.Net;

namespace JMAP.Net.Capabilities.Calendars.Types;

/// <summary>
/// Represents a JMAP Calendar - a named collection of events.
/// As per JMAP Calendars RFC, Section 4.
/// </summary>
public class Calendar
{
    /// <summary>
    /// The id of the calendar.
    /// </summary>
    [JsonPropertyName("id")]
    public required JmapId Id { get; init; }

    /// <summary>
    /// The user-visible name of the calendar.
    /// Must be at least 1 character and maximum 255 octets in size.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// An optional longer-form description of the calendar.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    /// <summary>
    /// A color to be used when displaying events associated with the calendar.
    /// Must be a CSS color name or RGB value in hexadecimal notation.
    /// </summary>
    [JsonPropertyName("color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Color { get; init; }

    /// <summary>
    /// Defines the sort order of calendars when presented in the client's UI.
    /// Must be an integer in the range 0 <= sortOrder < 2^31.
    /// </summary>
    [JsonPropertyName("sortOrder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JmapUnsignedInt SortOrder { get; init; } = new(0);

    /// <summary>
    /// True if the user has indicated they wish to see this Calendar in their client.
    /// Should default to false for shared calendars and true for user-created calendars.
    /// </summary>
    [JsonPropertyName("isSubscribed")]
    public required bool IsSubscribed { get; init; }

    /// <summary>
    /// Should the calendar's events be displayed to the user at the moment?
    /// Clients must ignore this if isSubscribed is false.
    /// Default: true.
    /// </summary>
    [JsonPropertyName("isVisible")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsVisible { get; init; } = true;

    /// <summary>
    /// Server-set. True for exactly one calendar in any account.
    /// The default calendar should be used when the client needs to choose a calendar.
    /// </summary>
    [JsonPropertyName("isDefault")]
    public required bool IsDefault { get; init; }

    /// <summary>
    /// Should the calendar's events be used as part of availability calculation?
    /// Values: "all", "attending", "none".
    /// </summary>
    [JsonPropertyName("includeInAvailability")]
    public required string IncludeInAvailability { get; init; }

    /// <summary>
    /// A map of alert ids to Alert objects to apply for events where showWithoutTime is false
    /// and useDefaultAlerts is true.
    /// </summary>
    [JsonPropertyName("defaultAlertsWithTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, Alert>? DefaultAlertsWithTime { get; init; }

    /// <summary>
    /// A map of alert ids to Alert objects to apply for events where showWithoutTime is true
    /// and useDefaultAlerts is true.
    /// </summary>
    [JsonPropertyName("defaultAlertsWithoutTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, Alert>? DefaultAlertsWithoutTime { get; init; }

    /// <summary>
    /// The time zone to use for events without a time zone.
    /// Must be an IANA Time Zone Database id.
    /// If null, the timeZone of the account's associated Principal is used.
    /// </summary>
    [JsonPropertyName("timeZone")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TimeZone { get; init; }

    /// <summary>
    /// A map of Principal id to rights for principals this calendar is shared with.
    /// Null if not shared with anyone.
    /// </summary>
    [JsonPropertyName("shareWith")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, CalendarRights>? ShareWith { get; init; }

    /// <summary>
    /// Server-set. The set of access rights the user has in relation to this Calendar.
    /// </summary>
    [JsonPropertyName("myRights")]
    public required CalendarRights MyRights { get; init; }
}
