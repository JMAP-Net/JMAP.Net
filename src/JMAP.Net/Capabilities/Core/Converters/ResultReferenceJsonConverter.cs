using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Converters;

/// <summary>
/// JSON converter for ResultReference that serializes as a string in the format: #methodCallId/path
/// As per RFC 8620, Section 3.7.
/// </summary>
public sealed class ResultReferenceJsonConverter : JsonConverter<ResultReference>
{
    /// <summary>
    /// Reads a <see cref="ResultReference" /> from its object representation.
    /// </summary>
    /// <param name="reader">The reader positioned at the JSON value.</param>
    /// <param name="typeToConvert">The target type being converted.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The deserialized <see cref="ResultReference" /> value.</returns>
    public override ResultReference Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Result reference must be an object with 'resultOf', 'name', and 'path' properties");

        // Read object notation: { "resultOf": "c1", "name": "Calendar/get", "path": "/list/0/id" }
        string? resultOf = null;
        string? name = null;
        string? path = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "resultOf":
                        resultOf = reader.GetString();
                        break;
                    case "name":
                        name = reader.GetString();
                        break;
                    case "path":
                        path = reader.GetString();
                        break;
                }
            }
        }

        if (string.IsNullOrEmpty(resultOf) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(path))
            throw new JsonException("Result reference must have 'resultOf', 'name', and 'path' properties");

        return new ResultReference
        {
            ResultOf = resultOf,
            Name = name,
            Path = path
        };
    }

    /// <summary>
    /// Writes a <see cref="ResultReference" /> as an object with <c>resultOf</c>,
    /// <c>name</c>, and <c>path</c> properties.
    /// </summary>
    /// <param name="writer">The writer to write JSON to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, ResultReference value, JsonSerializerOptions options)
    {
        // Write as object: { "resultOf": "c1", "name": "Calendar/get", "path": "/list/0/id" }
        writer.WriteStartObject();
        writer.WriteString("resultOf", value.ResultOf);
        writer.WriteString("name", value.Name);
        writer.WriteString("path", value.Path);
        writer.WriteEndObject();
    }
}
