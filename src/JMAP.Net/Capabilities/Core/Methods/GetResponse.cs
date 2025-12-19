using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Base response class for Foo/get methods.
/// As per RFC 8620, Section 5.1.
/// </summary>
/// <typeparam name="TObject">The type of object being fetched</typeparam>
public abstract class GetResponse<TObject>
{
    /// <summary>
    /// The id of the account used for the call.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// A string representing the state on the server for all data of this type in the account.
    /// If the data changes, this string MUST change.
    /// </summary>
    [JsonPropertyName("state")]
    public required string State { get; init; }

    /// <summary>
    /// An array of the Foo objects requested.
    /// This is the empty array if no objects were found or if the ids argument was empty.
    /// </summary>
    [JsonPropertyName("list")]
    public required List<TObject> List { get; init; }

    /// <summary>
    /// This array contains the ids passed to the method for records that do not exist.
    /// The array is empty if all requested ids were found or if ids was null/empty.
    /// </summary>
    [JsonPropertyName("notFound")]
    public required List<JmapId> NotFound { get; init; }
}

/// <summary>
/// Generic Foo/get response.
/// </summary>
public class GetResponse : GetResponse<object>
{
}
