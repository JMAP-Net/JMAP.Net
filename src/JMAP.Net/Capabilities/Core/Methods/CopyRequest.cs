using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Base request class for Foo/copy methods.
/// Allows copying records between two different accounts.
/// As per RFC 8620, Section 5.4.
/// </summary>
/// <typeparam name="TObject">The type of object being copied</typeparam>
public abstract class CopyRequest<TObject>
{
    /// <summary>
    /// The id of the account to copy records from.
    /// </summary>
    [JsonPropertyName("fromAccountId")]
    public required JmapId FromAccountId { get; init; }

    /// <summary>
    /// A state string as returned by Foo/get for the fromAccount.
    /// If supplied, the string must match the current state when reading data to be copied.
    /// If null, data will be read from the current state.
    /// </summary>
    [JsonPropertyName("ifFromInState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IfFromInState { get; init; }

    /// <summary>
    /// The id of the account to copy records to.
    /// This MUST be different from fromAccountId.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// A state string as returned by Foo/get for the destination account.
    /// If supplied, the string must match the current state of the accountId.
    /// If null, any changes will be applied to the current state.
    /// </summary>
    [JsonPropertyName("ifInState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IfInState { get; init; }

    /// <summary>
    /// A map of creation id to Foo object.
    /// The Foo object MUST contain an id property, which is the id (in the fromAccount)
    /// of the record to be copied.
    /// </summary>
    [JsonPropertyName("create")]
    public required Dictionary<JmapId, TObject> Create { get; init; }

    /// <summary>
    /// If true, an attempt will be made to destroy the original records that were
    /// successfully copied.
    /// Default: false.
    /// </summary>
    [JsonPropertyName("onSuccessDestroyOriginal")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool OnSuccessDestroyOriginal { get; init; } = false;

    /// <summary>
    /// This argument is passed as the ifInState argument to the implicit Foo/set call
    /// if onSuccessDestroyOriginal is true.
    /// </summary>
    [JsonPropertyName("destroyFromIfInState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DestroyFromIfInState { get; init; }
}

/// <summary>
/// Generic Foo/copy request.
/// </summary>
public class CopyRequest : CopyRequest<object>
{
}
