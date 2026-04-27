using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JSCalendar.Net;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Base request class for <c>*/set</c> methods.
/// This encompasses creating, updating, and destroying records of a single object type.
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
    /// A state string as returned by the corresponding <c>*/get</c> method.
    /// If supplied, the string must match the current state; otherwise, the method is aborted.
    /// If <see langword="null" />, any changes are applied to the current state.
    /// </summary>
    [JsonPropertyName("ifInState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IfInState { get; init; }

    /// <summary>
    /// A map of creation id (temporary id set by the client) to objects to create.
    /// <see langword="null" /> if no objects are to be created.
    /// </summary>
    [JsonPropertyName("create")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, TObject>? Create { get; init; }

    /// <summary>
    /// A map of id to a patch object to apply to the current object with that id.
    /// <see langword="null" /> if no objects are to be updated.
    /// </summary>
    [JsonPropertyName("update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, TPatch>? Update { get; init; }

    /// <summary>
    /// A list of ids for objects to permanently delete.
    /// <see langword="null" /> if no objects are to be destroyed.
    /// </summary>
    [JsonPropertyName("destroy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<JmapId>? Destroy { get; init; }
}

/// <summary>
/// Generic <c>*/set</c> request with dictionary-based patch objects.
/// </summary>
public sealed class SetRequest : SetRequest<object, PatchObject>
{
}
