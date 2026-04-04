using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Base response class for <c>*/changes</c> methods.
/// As per RFC 8620, Section 5.2.
/// </summary>
public class ChangesResponse
{
    /// <summary>
    /// The id of the account used for the call.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// This is the sinceState argument echoed back.
    /// It's the state from which the server is returning changes.
    /// </summary>
    [JsonPropertyName("oldState")]
    public required string OldState { get; init; }

    /// <summary>
    /// This is the state the client will be in after applying the set of changes to the old state.
    /// </summary>
    [JsonPropertyName("newState")]
    public required string NewState { get; init; }

    /// <summary>
    /// If <see langword="true" />, the client may call the corresponding <c>*/changes</c>
    /// method again with the returned <see cref="NewState" /> to get further updates.
    /// If <see langword="false" />, <see cref="NewState" /> is the current server state.
    /// </summary>
    [JsonPropertyName("hasMoreChanges")]
    public required bool HasMoreChanges { get; init; }

    /// <summary>
    /// An array of ids for records that have been created since the old state.
    /// </summary>
    [JsonPropertyName("created")]
    public required List<JmapId> Created { get; init; }

    /// <summary>
    /// An array of ids for records that have been updated since the old state.
    /// </summary>
    [JsonPropertyName("updated")]
    public required List<JmapId> Updated { get; init; }

    /// <summary>
    /// An array of ids for records that have been destroyed since the old state.
    /// </summary>
    [JsonPropertyName("destroyed")]
    public required List<JmapId> Destroyed { get; init; }
}
