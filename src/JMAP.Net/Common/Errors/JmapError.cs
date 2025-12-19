using System.Text.Json.Serialization;

namespace JMAP.Net.Common.Errors;

/// <summary>
/// Represents a JMAP error at the method level.
/// As per RFC 8620, Section 3.6.2.
/// </summary>
public class JmapError
{
    /// <summary>
    /// The error type identifier.
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    /// <summary>
    /// A description of the error to help with debugging.
    /// This is a non-localized string and is not intended to be shown to end users.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    /// <summary>
    /// Additional properties specific to certain error types.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, object?>? ExtensionData { get; init; }
}