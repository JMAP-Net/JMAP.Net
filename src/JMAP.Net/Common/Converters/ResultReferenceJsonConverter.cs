using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Common.Converters;

/// <summary>
/// JSON converter for ResultReference that serializes as a string in the format: #methodCallId/path
/// As per RFC 8620, Section 3.7.
/// </summary>
public class ResultReferenceJsonConverter : JsonConverter<ResultReference>
{
    public override ResultReference Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Result reference must be an object with 'resultOf', 'name', and 'path' properties");

        // Read object notation: { "resultOf": "c1", "name": "Foo/get", "path": "/list/0/id" }
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

    public override void Write(Utf8JsonWriter writer, ResultReference value, JsonSerializerOptions options)
    {
        // Write as object: { "resultOf": "c1", "name": "Foo/get", "path": "/list/0/id" }
        writer.WriteStartObject();
        writer.WriteString("resultOf", value.ResultOf);
        writer.WriteString("name", value.Name);
        writer.WriteString("path", value.Path);
        writer.WriteEndObject();
    }
}