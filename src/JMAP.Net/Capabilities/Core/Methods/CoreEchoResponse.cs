using System.Text.Json.Serialization;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Response for Core/echo method.
/// Contains the exact same arguments that were sent in the request.
/// </summary>
public class CoreEchoResponse
{
    /// <summary>
    /// The same arguments that were sent in the request.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, object?>? Arguments { get; init; }
}
