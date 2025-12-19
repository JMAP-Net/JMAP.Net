using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods.Query;

/// <summary>
/// Base response class for Foo/query methods.
/// As per RFC 8620, Section 5.5.
/// </summary>
public class QueryResponse
{
    /// <summary>
    /// The id of the account used for the call.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// A string encoding the current state of the query on the server.
    /// This string MUST change if the results of the query have changed.
    /// </summary>
    [JsonPropertyName("queryState")]
    public required string QueryState { get; init; }

    /// <summary>
    /// True if the server supports calling Foo/queryChanges with these filter/sort parameters.
    /// </summary>
    [JsonPropertyName("canCalculateChanges")]
    public required bool CanCalculateChanges { get; init; }

    /// <summary>
    /// The zero-based index of the first result in the ids array within the complete list of query results.
    /// </summary>
    [JsonPropertyName("position")]
    public required JmapUnsignedInt Position { get; init; }

    /// <summary>
    /// The list of ids for each Foo in the query results.
    /// </summary>
    [JsonPropertyName("ids")]
    public required List<JmapId> Ids { get; init; }

    /// <summary>
    /// The total number of Foos in the results (given the filter).
    /// Only present if calculateTotal was true in the request.
    /// </summary>
    [JsonPropertyName("total")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUnsignedInt? Total { get; init; }

    /// <summary>
    /// The limit enforced by the server on the maximum number of results to return.
    /// Only returned if the server set a limit or used a different limit than given in the request.
    /// </summary>
    [JsonPropertyName("limit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUnsignedInt? Limit { get; init; }
}
