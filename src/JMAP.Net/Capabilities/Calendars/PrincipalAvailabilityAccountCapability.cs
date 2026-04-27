using System.Text.Json.Serialization;

namespace JMAP.Net.Capabilities.Calendars;

/// <summary>
/// Represents the JMAP Principals availability account capability.
/// As per JMAP Calendars RFC, Section 1.5.2.
/// </summary>
public sealed class PrincipalAvailabilityAccountCapability
{
    /// <summary>
    /// The capability URI for the JMAP Principals availability capability.
    /// </summary>
    public const string CapabilityUri = PrincipalAvailabilityCapability.CapabilityUri;

    /// <summary>
    /// The maximum duration over which the server is prepared to calculate availability in a single call.
    /// </summary>
    [JsonPropertyName("maxAvailabilityDuration")]
    public required string MaxAvailabilityDuration { get; init; }
}
