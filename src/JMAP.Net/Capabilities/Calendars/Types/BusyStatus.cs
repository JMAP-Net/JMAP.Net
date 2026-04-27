using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Calendars.Converters;

namespace JMAP.Net.Capabilities.Calendars.Types;

/// <summary>
/// Defines the scheduling availability status for a busy period.
/// </summary>
[JsonConverter(typeof(BusyStatusJsonConverter))]
public enum BusyStatus
{
    /// <summary>
    /// The Principal is not available for scheduling at this time for any other reason.
    /// </summary>
    Unavailable,

    /// <summary>
    /// The event status is confirmed and the Principal's participation status is accepted.
    /// </summary>
    Confirmed,

    /// <summary>
    /// The event status is tentative or the Principal's participation status is tentative.
    /// </summary>
    Tentative
}
