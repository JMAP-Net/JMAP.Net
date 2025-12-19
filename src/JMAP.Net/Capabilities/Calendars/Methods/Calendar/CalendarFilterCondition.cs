using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Calendars.Methods.Calendar;

/// <summary>
/// A filter condition for Calendar/query.
/// All conditions in the filter must match for the Calendar to be included in results.
/// </summary>
public class CalendarFilterCondition
{
    /// <summary>
    /// The Calendar name contains this string (case-insensitive substring match).
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; init; }

    /// <summary>
    /// The Calendar is used for any of the data types in this set.
    /// e.g., "event", "todo" (although JMAP Calendars typically only uses "event").
    /// </summary>
    [JsonPropertyName("usedForDataTypes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? UsedForDataTypes { get; init; }

    /// <summary>
    /// The Calendar's isSubscribed value matches the given value.
    /// </summary>
    [JsonPropertyName("isSubscribed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsSubscribed { get; init; }

    /// <summary>
    /// The Calendar is shared with the given principal.
    /// </summary>
    [JsonPropertyName("sharedWith")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapId? SharedWith { get; init; }
}
