using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Methods;

/// <summary>
/// Base request class for Foo/get methods.
/// As per RFC 8620, Section 5.1.
/// </summary>
/// <typeparam name="TObject">The type of object being fetched</typeparam>
public abstract class GetRequest<TObject>
{
    /// <summary>
    /// The id of the account to use.
    /// </summary>
    [JsonPropertyName("accountId")]
    public required JmapId AccountId { get; init; }

    /// <summary>
    /// The ids of the Foo objects to return.
    /// If null, then all records of the data type are returned
    /// (if supported and within maxObjectsInGet limit).
    /// </summary>
    [JsonPropertyName("ids")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<JmapId>? Ids { get; init; }

    /// <summary>
    /// If supplied, only the properties listed are returned for each object.
    /// If null, all properties are returned.
    /// The id property is always returned, even if not explicitly requested.
    /// </summary>
    [JsonPropertyName("properties")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Properties { get; init; }
}

/// <summary>
/// Generic Foo/get request.
/// </summary>
public class GetRequest : GetRequest<object>
{
}
