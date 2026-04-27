using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Sharing.Types;

namespace JMAP.Net.Capabilities.Sharing.Converters;

/// <summary>
/// JSON converter for <see cref="PrincipalType" />.
/// </summary>
public sealed class PrincipalTypeJsonConverter : JsonConverter<PrincipalType>
{
    /// <inheritdoc />
    public override PrincipalType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "individual" => PrincipalType.Individual,
            "group" => PrincipalType.Group,
            "resource" => PrincipalType.Resource,
            "location" => PrincipalType.Location,
            "other" => PrincipalType.Other,
            var value => throw new JsonException($"Unknown principal type value '{value}'.")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, PrincipalType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            PrincipalType.Individual => "individual",
            PrincipalType.Group => "group",
            PrincipalType.Resource => "resource",
            PrincipalType.Location => "location",
            PrincipalType.Other => "other",
            _ => throw new JsonException($"Unknown principal type value '{value}'.")
        });
    }
}
