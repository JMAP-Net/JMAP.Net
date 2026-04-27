using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Converters;

/// <summary>
/// JSON converter for JmapUnsignedInt type.
/// </summary>
public sealed class JmapUnsignedIntJsonConverter : JsonConverter<JmapUnsignedInt>
{
    /// <summary>
    /// Reads a <see cref="JmapUnsignedInt" /> from a JSON number value.
    /// </summary>
    /// <param name="reader">The reader positioned at the JSON value.</param>
    /// <param name="typeToConvert">The target type being converted.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The deserialized <see cref="JmapUnsignedInt" /> value.</returns>
    public override JmapUnsignedInt Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException("Expected number token for JmapUnsignedInt");
        
        var value = reader.GetInt64();
        return new JmapUnsignedInt(value);
    }

    /// <summary>
    /// Writes a <see cref="JmapUnsignedInt" /> as a JSON number value.
    /// </summary>
    /// <param name="writer">The writer to write JSON to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, JmapUnsignedInt value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}
