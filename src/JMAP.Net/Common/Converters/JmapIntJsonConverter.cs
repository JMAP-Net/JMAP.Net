using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Common.Converters;

/// <summary>
/// JSON converter for JmapInt type.
/// </summary>
public class JmapIntJsonConverter : JsonConverter<JmapInt>
{
    /// <summary>
    /// Reads a <see cref="JmapInt" /> from a JSON number value.
    /// </summary>
    /// <param name="reader">The reader positioned at the JSON value.</param>
    /// <param name="typeToConvert">The target type being converted.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The deserialized <see cref="JmapInt" /> value.</returns>
    public override JmapInt Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException("Expected number token for JmapInt");

        var value = reader.GetInt64();
        return new JmapInt(value);
    }

    /// <summary>
    /// Writes a <see cref="JmapInt" /> as a JSON number value.
    /// </summary>
    /// <param name="writer">The writer to write JSON to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, JmapInt value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}
