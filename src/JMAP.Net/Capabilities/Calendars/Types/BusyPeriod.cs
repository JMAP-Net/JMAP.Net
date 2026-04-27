using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Calendars.Types;

/// <summary>
/// Represents a period of time in which a Principal is busy.
/// As per JMAP Calendars RFC, Section 2.2.
/// </summary>
public sealed class BusyPeriod
{
    /// <summary>
    /// The start time, inclusive, of the period this represents.
    /// </summary>
    [JsonPropertyName("utcStart")]
    public required JmapUtcDate UtcStart { get; init; }

    /// <summary>
    /// The end time, exclusive, of the period this represents.
    /// </summary>
    [JsonPropertyName("utcEnd")]
    public required JmapUtcDate UtcEnd { get; init; }

    /// <summary>
    /// The busy status for the period.
    /// </summary>
    [JsonPropertyName("busyStatus")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public BusyStatus BusyStatus { get; init; } = BusyStatus.Unavailable;

    /// <summary>
    /// The CalendarEvent representation of the event, or null if details are unavailable.
    /// </summary>
    [JsonPropertyName("event")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CalendarEvent? Event { get; init; }

    /// <summary>
    /// The account id in which the event can be found, or null if the event property is null.
    /// </summary>
    [JsonPropertyName("accountId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapId? AccountId { get; init; }
}
