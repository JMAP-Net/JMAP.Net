using System.Text.Json.Serialization;

namespace JMAP.Net.Common.Errors;

/// <summary>
/// Represents a SetError for failed create/update/destroy operations in /set methods.
/// As per RFC 8620, Section 5.3.
/// </summary>
public class SetError
{
    /// <summary>
    /// The type of error.
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
    /// For invalidProperties errors: list of all the properties that were invalid.
    /// </summary>
    [JsonPropertyName("properties")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Properties { get; init; }

    /// <summary>
    /// For alreadyExists errors in /copy: the id of the existing record.
    /// </summary>
    [JsonPropertyName("existingId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExistingId { get; init; }

    /// <summary>
    /// Additional properties specific to certain error types.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, object?>? ExtensionData { get; init; }
}