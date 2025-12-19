using System.Text.Json.Serialization;
using JMAP.Net.Common.Converters;

namespace JMAP.Net.Capabilities.Core.Types;

/// <summary>
/// Represents a result reference to allow clients to use the result of one method call
/// as input to a subsequent method call.
/// As per RFC 8620, Section 3.7.
/// </summary>
/// <remarks>
/// To use a result reference, the client prefixes the argument name with "#".
/// The value is a ResultReference object with the following properties:
/// 
/// Example usage in a request:
/// <code>
/// {
///   "#ids": {
///     "resultOf": "c1",
///     "name": "Foo/query",
///     "path": "/ids"
///   }
/// }
/// </code>
/// 
/// The server resolves the reference and replaces it with the actual value before processing.
/// </remarks>
[JsonConverter(typeof(ResultReferenceJsonConverter))]
public class ResultReference
{
    /// <summary>
    /// The method call id (see RFC 8620, Section 3.2) of a previous method call in the current request.
    /// This is the third element in the Invocation array.
    /// </summary>
    [JsonPropertyName("resultOf")]
    public required string ResultOf { get; init; }

    /// <summary>
    /// The required name of a response to that method call.
    /// This must match the first element in the response Invocation array.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// A JSON Pointer (RFC 6901) into the arguments of the response selected via name and resultOf.
    /// The pointer has an implicit leading "/" (i.e., prefix each key with "/" before applying JSON Pointer).
    /// Additionally supports "*" to map through an array (see RFC 8620, Section 3.7).
    /// </summary>
    /// <remarks>
    /// The "*" token applies the rest of the pointer to every item in an array and flattens the results.
    /// 
    /// Example: "list/*/threadId" applied to:
    /// <code>
    /// {
    ///   "list": [
    ///     { "id": "a", "threadId": "t1" },
    ///     { "id": "b", "threadId": "t2" }
    ///   ]
    /// }
    /// </code>
    /// 
    /// Results in: ["t1", "t2"]
    /// </remarks>
    [JsonPropertyName("path")]
    public required string Path { get; init; }

    /// <summary>
    /// Creates a string representation of this result reference.
    /// Note: This is for debugging; the actual JSON serialization format is an object.
    /// </summary>
    public override string ToString()
    {
        return $"ResultReference {{ resultOf: \"{ResultOf}\", name: \"{Name}\", path: \"{Path}\" }}";
    }
}
