using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Calendars.Methods.Principal;

/// <summary>
/// Request for Principal/getAvailability method.
/// As per JMAP Calendars RFC, Section 2.2.
/// </summary>
public sealed class PrincipalGetAvailabilityRequest
{
    /// <summary>
    /// The id of the account to use.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// The id of the Principal to calculate availability for.
    /// </summary>
    [JsonPropertyName("id")]
    public required JmapId Id { get; init; }

    /// <summary>
    /// The start time, inclusive, of the period for which to return availability.
    /// </summary>
    [JsonPropertyName("utcStart")]
    public required JmapUtcDate UtcStart { get; init; }

    /// <summary>
    /// The end time, exclusive, of the period for which to return availability.
    /// </summary>
    [JsonPropertyName("utcEnd")]
    public required JmapUtcDate UtcEnd { get; init; }

    /// <summary>
    /// If true, event details will be returned if the user has permission to view them.
    /// </summary>
    [JsonPropertyName("showDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ShowDetails { get; init; }

    /// <summary>
    /// A list of CalendarEvent properties to include in returned event details, or null for all properties.
    /// </summary>
    [JsonPropertyName("eventProperties")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? EventProperties { get; init; }
}
