using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods.Query;

/// <summary>
/// Base request class for Foo/queryChanges methods.
/// As per RFC 8620, Section 5.6.
/// </summary>
/// <typeparam name="TFilter">The type of filter condition</typeparam>
public abstract class QueryChangesRequest<TFilter>
{
    /// <summary>
    /// The id of the account to use.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// The filter argument that was used with Foo/query.
    /// </summary>
    [JsonPropertyName("filter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Filter { get; init; }  // Can be FilterOperator or TFilter

    /// <summary>
    /// The sort argument that was used with Foo/query.
    /// </summary>
    [JsonPropertyName("sort")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Comparator>? Sort { get; init; }

    /// <summary>
    /// The current state of the query in the client.
    /// This is the string returned as queryState in the Foo/query response.
    /// </summary>
    [JsonPropertyName("sinceQueryState")]
    public required string SinceQueryState { get; init; }

    /// <summary>
    /// The maximum number of changes to return in the response.
    /// </summary>
    [JsonPropertyName("maxChanges")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUnsignedInt? MaxChanges { get; init; }

    /// <summary>
    /// The last (highest-index) id the client currently has cached from the query results.
    /// If the sort and filter are both only on immutable properties, this allows the server
    /// to omit changes after this point in the results.
    /// </summary>
    [JsonPropertyName("upToId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapId? UpToId { get; init; }

    /// <summary>
    /// Does the client wish to know the total number of results now in the query?
    /// Default: false.
    /// </summary>
    [JsonPropertyName("calculateTotal")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool CalculateTotal { get; init; } = false;
}

/// <summary>
/// Generic Foo/queryChanges request.
/// </summary>
public class QueryChangesRequest : QueryChangesRequest<object>
{
}
