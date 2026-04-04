using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Base response class for <c>*/copy</c> methods.
/// As per RFC 8620, Section 5.4.
/// </summary>
/// <typeparam name="TObject">The type of object being copied</typeparam>
public abstract class CopyResponse<TObject>
{
    /// <summary>
    /// The id of the account records were copied from.
    /// </summary>
    [JsonPropertyName("fromAccountId")]
    public required JmapId FromAccountId { get; init; }

    /// <summary>
    /// The id of the account records were copied to.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// The state string that would have been returned by the corresponding <c>*/get</c>
    /// method on the destination account before making the requested changes.
    /// <see langword="null" /> if the server does not know the previous state string.
    /// </summary>
    [JsonPropertyName("oldState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OldState { get; init; }

    /// <summary>
    /// The state string that will now be returned by the corresponding <c>*/get</c>
    /// method on the destination account.
    /// </summary>
    [JsonPropertyName("newState")]
    public required string NewState { get; init; }

    /// <summary>
    /// A map of creation id to an object containing properties of the copied record
    /// that are set by the server (such as the id).
    /// The id is likely to be different from the id of the object in the source account.
    /// <see langword="null" /> if no records were successfully copied.
    /// </summary>
    [JsonPropertyName("created")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, TObject>? Created { get; init; }

    /// <summary>
    /// A map of creation id to <see cref="SetError" /> for each record that failed to be copied.
    /// <see langword="null" /> if none.
    /// </summary>
    [JsonPropertyName("notCreated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, SetError>? NotCreated { get; init; }
}

/// <summary>
/// Generic <c>*/copy</c> response.
/// </summary>
public class CopyResponse : CopyResponse<object>
{
}
