using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods.Query;

/// <summary>
/// Represents an item added to query results.
/// </summary>
public class AddedItem
{
    /// <summary>
    /// The id of the added item.
    /// </summary>
    [JsonPropertyName("id")]
    public required JmapId Id { get; init; }

    /// <summary>
    /// The index in the query results where this item appears.
    /// </summary>
    [JsonPropertyName("index")]
    public required JmapUnsignedInt Index { get; init; }
}