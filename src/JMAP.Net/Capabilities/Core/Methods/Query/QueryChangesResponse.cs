using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods.Query;

/// <summary>
/// Base response class for Foo/queryChanges methods.
/// As per RFC 8620, Section 5.6.
/// </summary>
public class QueryChangesResponse
{
    /// <summary>
    /// The id of the account used for the call.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// This is the sinceQueryState argument echoed back.
    /// The state from which the server is returning changes.
    /// </summary>
    [JsonPropertyName("oldQueryState")]
    public required string OldQueryState { get; init; }

    /// <summary>
    /// This is the state the query will be in after applying the set of changes to the old state.
    /// </summary>
    [JsonPropertyName("newQueryState")]
    public required string NewQueryState { get; init; }

    /// <summary>
    /// The total number of Foos in the results (given the filter).
    /// Only present if calculateTotal was true in the request.
    /// </summary>
    [JsonPropertyName("total")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JmapUnsignedInt? Total { get; init; }

    /// <summary>
    /// The id for every Foo that was in the query results in the old state
    /// and is not in the results in the new state.
    /// </summary>
    [JsonPropertyName("removed")]
    public required List<JmapId> Removed { get; init; }

    /// <summary>
    /// The id and index in the query results (in the new state) for every Foo that has been
    /// added to the results since the old state.
    /// The array MUST be sorted in order of index, lowest first.
    /// </summary>
    [JsonPropertyName("added")]
    public required List<AddedItem> Added { get; init; }
}