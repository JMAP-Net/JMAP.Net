using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JSCalendar.Net;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Base request class for Foo/set methods.
/// This encompasses creating, updating, and destroying Foo records.
/// As per RFC 8620, Section 5.3.
/// </summary>
/// <typeparam name="TObject">The type of object being modified</typeparam>
/// <typeparam name="TPatch">The type of patch object for updates</typeparam>
public abstract class SetRequest<TObject, TPatch>
{
    /// <summary>
    /// The id of the account to use.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// This is a state string as returned by Foo/get.
    /// If supplied, the string must match the current state; otherwise, the method will be aborted.
    /// If null, any changes will be applied to the current state.
    /// </summary>
    [JsonPropertyName("ifInState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IfInState { get; init; }

    /// <summary>
    /// A map of creation id (temporary id set by client) to Foo objects to create.
    /// Null if no objects are to be created.
    /// </summary>
    [JsonPropertyName("create")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, TObject>? Create { get; init; }

    /// <summary>
    /// A map of id to a Patch object to apply to the current Foo object with that id.
    /// Null if no objects are to be updated.
    /// </summary>
    [JsonPropertyName("update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, TPatch>? Update { get; init; }

    /// <summary>
    /// A list of ids for Foo objects to permanently delete.
    /// Null if no objects are to be destroyed.
    /// </summary>
    [JsonPropertyName("destroy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<JmapId>? Destroy { get; init; }
}

/// <summary>
/// Generic Foo/set request with dictionary-based patch objects.
/// </summary>
public class SetRequest : SetRequest<object, PatchObject>
{
}