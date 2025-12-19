using System.Text.Json.Serialization;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// The Core/echo method returns exactly the same arguments as it is given.
/// It is useful for testing if you have a valid authenticated connection to a JMAP API endpoint.
/// As per RFC 8620, Section 4.
/// </summary>
public class CoreEchoRequest
{
    /// <summary>
    /// Arguments to echo back. Can contain any valid JSON data.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, object?>? Arguments { get; init; }
}