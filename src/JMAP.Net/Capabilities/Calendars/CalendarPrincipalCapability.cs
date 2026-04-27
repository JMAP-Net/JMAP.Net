using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Calendars;

/// <summary>
/// Represents the JMAP Calendars capability entry on a Principal.
/// As per JMAP Calendars RFC, Section 2.1.
/// </summary>
public sealed class CalendarPrincipalCapability
{
    /// <summary>
    /// The capability URI for the JMAP Calendars capability.
    /// </summary>
    public const string CapabilityUri = CalendarCapability.CapabilityUri;

    /// <summary>
    /// Id of the account with calendar data for this Principal, or null if none is available to the user.
    /// </summary>
    [JsonPropertyName("accountId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapId? AccountId { get; init; }

    /// <summary>
    /// If true, the user may call Principal/getAvailability with this Principal.
    /// </summary>
    [JsonPropertyName("mayGetAvailability")]
    public required bool MayGetAvailability { get; init; }

    /// <summary>
    /// If true, the user may add this Principal as a calendar share target.
    /// </summary>
    [JsonPropertyName("mayShareWith")]
    public required bool MayShareWith { get; init; }

    /// <summary>
    /// The calendar address to use when this Principal may be added as a participant to an event.
    /// </summary>
    [JsonPropertyName("calendarAddress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CalendarAddress { get; init; }
}
