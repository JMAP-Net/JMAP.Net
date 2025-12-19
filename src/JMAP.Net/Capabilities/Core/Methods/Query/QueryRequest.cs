using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods.Query;

/// <summary>
/// Base request class for Foo/query methods.
/// As per RFC 8620, Section 5.5.
/// </summary>
/// <typeparam name="TFilter">The type of filter condition</typeparam>
public abstract class QueryRequest<TFilter>
{
    /// <summary>
    /// The id of the account to use.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// Determines the set of Foos returned in the results.
    /// If null, all objects in the account of this type are included.
    /// </summary>
    [JsonPropertyName("filter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Filter { get; init; }  // Can be FilterOperator or TFilter

    /// <summary>
    /// Lists how to compare Foo records to determine sort order.
    /// If null or empty, the sort order is server-dependent but must be stable.
    /// </summary>
    [JsonPropertyName("sort")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Comparator>? Sort { get; init; }

    /// <summary>
    /// The zero-based index of the first id in the full list of results to return.
    /// Default: 0.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JmapInt Position { get; init; } = new(0);

    /// <summary>
    /// A Foo id. If supplied, the position argument is ignored and the index of this id
    /// in the results is used instead.
    /// </summary>
    [JsonPropertyName("anchor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapId? Anchor { get; init; }

    /// <summary>
    /// The index of the first result to return relative to the anchor, if an anchor is given.
    /// May be negative.
    /// Default: 0.
    /// </summary>
    [JsonPropertyName("anchorOffset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JmapInt AnchorOffset { get; init; } = new(0);

    /// <summary>
    /// The maximum number of results to return.
    /// If null, no limit presumed.
    /// </summary>
    [JsonPropertyName("limit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUnsignedInt? Limit { get; init; }

    /// <summary>
    /// Does the client wish to know the total number of results in the query?
    /// Default: false.
    /// </summary>
    [JsonPropertyName("calculateTotal")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool CalculateTotal { get; init; } = false;
}

/// <summary>
/// Generic Foo/query request.
/// </summary>
public class QueryRequest : QueryRequest<object>
{
}
