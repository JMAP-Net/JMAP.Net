using System.Text.Json.Serialization;
using JMAP.Net.Common.Converters;

namespace JMAP.Net.Common.Protocol;

/// <summary>
/// Represents a JMAP method call or response invocation.
/// An invocation is a tuple of [name, arguments, methodCallId].
/// As per RFC 8620, Section 3.2.
/// </summary>
[JsonConverter(typeof(InvocationJsonConverter))]
public class Invocation
{
    /// <summary>
    /// The name of the method to call or the response name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Named arguments for the method or response.
    /// </summary>
    public required Dictionary<string, object?> Arguments { get; init; }

    /// <summary>
    /// An arbitrary string from the client echoed back with responses.
    /// </summary>
    public required string MethodCallId { get; init; }
}