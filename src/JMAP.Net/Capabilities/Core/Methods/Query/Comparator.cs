using System.Text.Json.Serialization;

namespace JMAP.Net.Capabilities.Core.Methods.Query;

/// <summary>
/// Specifies how to compare properties when sorting query results.
/// As per RFC 8620, Section 5.5.
/// </summary>
public class Comparator
{
    /// <summary>
    /// The name of the property on the objects being queried to compare.
    /// </summary>
    [JsonPropertyName("property")]
    public required string Property { get; init; }

    /// <summary>
    /// If true, sort in ascending order. If false, sort in descending order.
    /// The model initializes this property to <see langword="true" />.
    /// Serialization note: because <see cref="JsonIgnoreCondition.WhenWritingDefault" />
    /// uses the CLR default for <see cref="bool" />, <c>false</c> is omitted from JSON
    /// while <c>true</c> is emitted explicitly.
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
