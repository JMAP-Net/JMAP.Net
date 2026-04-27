using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Common.Protocol;

namespace JMAP.Net.Common.Converters;

/// <summary>
/// JSON converter for Invocation that serializes as a JSON array [name, arguments, methodCallId].
/// </summary>
public sealed class InvocationJsonConverter : JsonConverter<Invocation>
{
    /// <summary>
    /// Reads an <see cref="Invocation" /> from its compact JSON array representation.
    /// </summary>
    /// <param name="reader">The reader positioned at the JSON value.</param>
    /// <param name="typeToConvert">The target type being converted.</param>
    /// <param name="options">The serializer options to use for nested values.</param>
    /// <returns>The deserialized <see cref="Invocation" /> instance.</returns>
    public override Invocation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected array for Invocation");

        reader.Read();
        var name = reader.GetString() ?? throw new JsonException("Invocation name cannot be null");

        reader.Read();
        var arguments = JsonSerializer.Deserialize<Dictionary<string, object?>>(ref reader, options)
                        ?? throw new JsonException("Invocation arguments cannot be null");

        reader.Read();
        var methodCallId = reader.GetString() ?? throw new JsonException("Invocation methodCallId cannot be null");

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndArray)
            throw new JsonException("Expected end of array for Invocation");

        return new Invocation
        {
            Name = name,
            Arguments = arguments,
            MethodCallId = methodCallId
        };
    }

    /// <summary>
    /// Writes an <see cref="Invocation" /> as the compact JSON array representation
    /// <c>[name, arguments, methodCallId]</c>.
    /// </summary>
    /// <param name="writer">The writer to write JSON to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">The serializer options to use for nested values.</param>
    public override void Write(Utf8JsonWriter writer, Invocation value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteStringValue(value.Name);
        JsonSerializer.Serialize(writer, value.Arguments, options);
        writer.WriteStringValue(value.MethodCallId);
        writer.WriteEndArray();
    }
}
