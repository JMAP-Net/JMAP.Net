using System.Text.Json.Serialization;

namespace JMAP.Net.Common.Errors;

/// <summary>
/// Represents a JSON problem details object as per RFC 7807, used for request-level errors.
/// As per RFC 8620, Section 3.6.1.
/// </summary>
public class ProblemDetails
{
    /// <summary>
    /// A URI reference that identifies the problem type.
    /// For JMAP errors, this uses the format: urn:ietf:params:jmap:error:{errorType}
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    /// <summary>
    /// The HTTP status code for this occurrence of the problem.
    /// </summary>
    [JsonPropertyName("status")]
    public required int Status { get; init; }

    /// <summary>
    /// A human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    [JsonPropertyName("detail")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Detail { get; init; }

    /// <summary>
    /// For limit errors: the name of the limit being applied.
    /// </summary>
    [JsonPropertyName("limit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Limit { get; init; }

    /// <summary>
    /// Additional properties specific to certain problem types.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, object?>? ExtensionData { get; init; }
}