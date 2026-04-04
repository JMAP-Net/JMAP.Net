using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Base response class for <c>*/set</c> methods.
/// As per RFC 8620, Section 5.3.
/// </summary>
/// <typeparam name="TObject">The type of object being modified</typeparam>
public abstract class SetResponse<TObject>
{
    /// <summary>
    /// The id of the account used for the call.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// The state string that would have been returned by the corresponding <c>*/get</c>
    /// method before making the requested changes.
    /// <see langword="null" /> if the server does not know the previous state string.
    /// </summary>
    [JsonPropertyName("oldState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OldState { get; init; }

    /// <summary>
    /// The state string that will now be returned by the corresponding <c>*/get</c> method.
    /// </summary>
    [JsonPropertyName("newState")]
    public required string NewState { get; init; }

    /// <summary>
    /// A map of creation id to an object containing properties of the created record
    /// that were not sent by the client, such as server-set properties.
    /// <see langword="null" /> if no records were successfully created.
    /// </summary>
    [JsonPropertyName("created")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, TObject>? Created { get; init; }

    /// <summary>
    /// The keys in this map are the ids of all records that were successfully updated.
    /// The value for each id is an object containing any property that changed
    /// in a way not explicitly requested by the patch object, or <see langword="null" />
    /// if there were no such changes.
    /// <see langword="null" /> if no records were successfully updated.
    /// </summary>
    [JsonPropertyName("updated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, TObject?>? Updated { get; init; }

    /// <summary>
    /// A list of ids for records that were successfully destroyed.
    /// <see langword="null" /> if none.
    /// </summary>
    [JsonPropertyName("destroyed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<JmapId>? Destroyed { get; init; }

    /// <summary>
    /// A map of creation id to <see cref="SetError" /> for each record that failed to be created.
    /// <see langword="null" /> if all creates succeeded.
    /// </summary>
    [JsonPropertyName("notCreated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, SetError>? NotCreated { get; init; }

    /// <summary>
    /// A map of record id to <see cref="SetError" /> for each record that failed to be updated.
    /// <see langword="null" /> if all updates succeeded.
    /// </summary>
    [JsonPropertyName("notUpdated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, SetError>? NotUpdated { get; init; }

    /// <summary>
    /// A map of record id to <see cref="SetError" /> for each record that failed to be destroyed.
    /// <see langword="null" /> if all destroys succeeded.
    /// </summary>
    [JsonPropertyName("notDestroyed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, SetError>? NotDestroyed { get; init; }
}

/// <summary>
/// Generic <c>*/set</c> response.
/// </summary>
public class SetResponse : SetResponse<object>
{
}
