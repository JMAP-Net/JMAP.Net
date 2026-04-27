using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Sharing.Converters;

namespace JMAP.Net.Capabilities.Sharing.Types;

/// <summary>
/// Defines the kind of entity represented by a Principal.
/// </summary>
[JsonConverter(typeof(PrincipalTypeJsonConverter))]
public enum PrincipalType
{
    /// <summary>
    /// A single person.
    /// </summary>
    Individual,

    /// <summary>
    /// A group of other Principals.
    /// </summary>
    Group,

    /// <summary>
    /// A shared resource, such as a projector.
    /// </summary>
    Resource,

    /// <summary>
    /// A location, such as a room.
    /// </summary>
    Location,

    /// <summary>
    /// Another undefined Principal type.
    /// </summary>
    Other
}
