using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Calendars.Converters;

namespace JMAP.Net.Capabilities.Calendars.Types;

/// <summary>
/// Defines how a calendar contributes to availability calculation.
/// </summary>
[JsonConverter(typeof(CalendarAvailabilityInclusionJsonConverter))]
public enum CalendarAvailabilityInclusion
{
    /// <summary>
    /// Include all events in availability calculation.
    /// </summary>
    All,

    /// <summary>
    /// Include events where the user is attending.
    /// </summary>
    Attending,

    /// <summary>
    /// Do not include this calendar in availability calculation.
    /// </summary>
    None
}
