using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Base response class for Foo/set methods.
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
    /// The state string that would have been returned by Foo/get before making the requested changes.
    /// Null if the server doesn't know what the previous state string was.
    /// </summary>
    [JsonPropertyName("oldState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OldState { get; init; }

    /// <summary>
    /// The state string that will now be returned by Foo/get.
    /// </summary>
    [JsonPropertyName("newState")]
    public required string NewState { get; init; }

    /// <summary>
    /// A map of creation id to an object containing properties of the created Foo object
    /// that were not sent by the client (e.g., server-set properties).
    /// Null if no Foo objects were successfully created.
    /// </summary>
    [JsonPropertyName("created")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, TObject>? Created { get; init; }

    /// <summary>
    /// The keys in this map are the ids of all Foos that were successfully updated.
    /// The value for each id is a Foo object containing any property that changed
    /// in a way not explicitly requested by the PatchObject, or null if none.
    /// Null if no Foo objects were successfully updated.
    /// </summary>
    [JsonPropertyName("updated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, TObject?>? Updated { get; init; }

    /// <summary>
    /// A list of Foo ids for records that were successfully destroyed.
    /// Null if none.
    /// </summary>
    [JsonPropertyName("destroyed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<JmapId>? Destroyed { get; init; }

    /// <summary>
    /// A map of creation id to SetError for each record that failed to be created.
    /// Null if all successful.
    /// </summary>
    [JsonPropertyName("notCreated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, SetError>? NotCreated { get; init; }

    /// <summary>
    /// A map of Foo id to SetError for each record that failed to be updated.
    /// Null if all successful.
    /// </summary>
    [JsonPropertyName("notUpdated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, SetError>? NotUpdated { get; init; }

    /// <summary>
    /// A map of Foo id to SetError for each record that failed to be destroyed.
    /// Null if all successful.
    /// </summary>
    [JsonPropertyName("notDestroyed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, SetError>? NotDestroyed { get; init; }
}

/// <summary>
/// Generic Foo/set response.
/// </summary>
public class SetResponse : SetResponse<object>
{
}
