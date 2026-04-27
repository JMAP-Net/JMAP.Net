using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Calendars.Types;

namespace JMAP.Net.Capabilities.Calendars.Methods.Principal;

/// <summary>
/// Response for Principal/getAvailability method.
/// As per JMAP Calendars RFC, Section 2.2.
/// </summary>
public sealed class PrincipalGetAvailabilityResponse
{
    /// <summary>
    /// The list of calculated busy periods.
    /// </summary>
    [JsonPropertyName("list")]
    public required List<BusyPeriod> List { get; init; }
}
