using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;
using JMAP.Net.Common.Errors;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Base response class for Foo/copy methods.
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
    /// The state string that would have been returned by Foo/get on the account records
    /// were copied to before making the requested changes.
    /// Null if the server doesn't know what the previous state string was.
    /// </summary>
    [JsonPropertyName("oldState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OldState { get; init; }

    /// <summary>
    /// The state string that will now be returned by Foo/get on the account records were copied to.
    /// </summary>
    [JsonPropertyName("newState")]
    public required string NewState { get; init; }

    /// <summary>
    /// A map of creation id to an object containing properties of the copied Foo object
    /// that are set by the server (such as the id).
    /// Note: the id is likely to be different from the id of the object in the account it was copied from.
    /// Null if no Foo objects were successfully copied.
    /// </summary>
    [JsonPropertyName("created")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, TObject>? Created { get; init; }

    /// <summary>
    /// A map of creation id to SetError for each record that failed to be copied.
    /// Null if none.
    /// </summary>
    [JsonPropertyName("notCreated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<JmapId, SetError>? NotCreated { get; init; }
}

/// <summary>
/// Generic Foo/copy response.
/// </summary>
public class CopyResponse : CopyResponse<object>
{
}
