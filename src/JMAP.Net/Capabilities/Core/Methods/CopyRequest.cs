using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Base request class for <c>*/copy</c> methods.
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
    /// A state string as returned by the corresponding <c>*/get</c> method for the source account.
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
    /// A state string as returned by the corresponding <c>*/get</c> method for the destination account.
    /// If supplied, the string must match the current state of the accountId.
    /// If null, any changes will be applied to the current state.
    /// </summary>
    [JsonPropertyName("ifInState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IfInState { get; init; }

    /// <summary>
    /// A map of creation id to an object to copy.
    /// Each value identifies the source record to copy by including its id from
    /// <see cref="FromAccountId" />.
    /// </summary>
    [JsonPropertyName("create")]
    public required Dictionary<JmapId, TObject> Create { get; init; }

    /// <summary>
    /// If <see langword="true" />, the server attempts to destroy any source records that
    /// were copied successfully.
    /// </summary>
    [JsonPropertyName("onSuccessDestroyOriginal")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool OnSuccessDestroyOriginal { get; init; } = false;

    /// <summary>
    /// This argument is passed as the <c>ifInState</c> argument to the implicit <c>*/set</c> call
    /// if onSuccessDestroyOriginal is true.
    /// </summary>
    [JsonPropertyName("destroyFromIfInState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DestroyFromIfInState { get; init; }
}

/// <summary>
/// Generic <c>*/copy</c> request.
/// </summary>
public class CopyRequest : CopyRequest<object>
{
}
