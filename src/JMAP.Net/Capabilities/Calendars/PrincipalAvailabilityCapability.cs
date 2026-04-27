namespace JMAP.Net.Capabilities.Calendars;

/// <summary>
/// Represents the JMAP Principals availability session capability.
/// As per JMAP Calendars RFC, Section 1.5.2.
/// URN: urn:ietf:params:jmap:principals:availability
/// </summary>
public sealed class PrincipalAvailabilityCapability
{
    /// <summary>
    /// The capability URI for the JMAP Principals availability capability.
    /// </summary>
    public const string CapabilityUri = "urn:ietf:params:jmap:principals:availability";
}
