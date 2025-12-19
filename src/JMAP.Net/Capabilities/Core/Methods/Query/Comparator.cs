using System.Text.Json.Serialization;

namespace JMAP.Net.Capabilities.Core.Methods.Query;

/// <summary>
/// Specifies how to compare properties when sorting query results.
/// As per RFC 8620, Section 5.5.
/// </summary>
public class Comparator
{
    /// <summary>
    /// The name of the property on the Foo objects to compare.
    /// </summary>
    [JsonPropertyName("property")]
    public required string Property { get; init; }

    /// <summary>
    /// If true, sort in ascending order. If false, sort in descending order.
    /// Default: true.
    /// </summary>
    [JsonPropertyName("isAscending")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsAscending { get; init; } = true;

    /// <summary>
    /// The collation algorithm identifier (as per RFC 4790) to use when comparing strings.
    /// If omitted, the default is server-dependent but must be unicode-aware.
    /// </summary>
    [JsonPropertyName("collation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Collation { get; init; }
}
