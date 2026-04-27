using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Capabilities.Core.Converters;

/// <summary>
/// JSON converter for JmapId type.
/// </summary>
public sealed class JmapIdJsonConverter : JsonConverter<JmapId>
{
    /// <summary>
    /// Reads a <see cref="JmapId" /> from a JSON string value.
    /// </summary>
    /// <param name="reader">The reader positioned at the JSON value.</param>
    /// <param name="typeToConvert">The target type being converted.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The deserialized <see cref="JmapId" /> value.</returns>
    public override JmapId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrEmpty(value) 
            ? throw new JsonException("JMAP Id cannot be null or empty")
            : new JmapId(value);
    }

    /// <summary>
    /// Writes a <see cref="JmapId" /> as a JSON string value.
    /// </summary>
    /// <param name="writer">The writer to write JSON to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, JmapId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }

    /// <summary>
    /// Reads a <see cref="JmapId" /> from a JSON property name.
    /// </summary>
    /// <param name="reader">The reader positioned at the property name.</param>
    /// <param name="typeToConvert">The target type being converted.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The deserialized <see cref="JmapId" /> value.</returns>
    public override JmapId ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrEmpty(value)
            ? throw new JsonException("JMAP Id property name cannot be null or empty")
            : new JmapId(value);
    }

    /// <summary>
    /// Writes a <see cref="JmapId" /> as a JSON property name.
    /// </summary>
    /// <param name="writer">The writer to write JSON to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">The serializer options.</param>
    public override void WriteAsPropertyName(Utf8JsonWriter writer, JmapId value, JsonSerializerOptions options)
    {
        writer.WritePropertyName(value.Value);
    }
}
