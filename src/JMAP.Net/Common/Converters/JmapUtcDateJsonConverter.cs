using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Common.Converters;

/// <summary>
/// JSON converter for JmapUtcDate type.
/// </summary>
public class JmapUtcDateJsonConverter : JsonConverter<JmapUtcDate>
{
    /// <summary>
    /// Reads a <see cref="JmapUtcDate" /> from a JSON string value.
    /// </summary>
    /// <param name="reader">The reader positioned at the JSON value.</param>
    /// <param name="typeToConvert">The target type being converted.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The deserialized <see cref="JmapUtcDate" /> value.</returns>
    public override JmapUtcDate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            throw new JsonException("JMAP UTCDate cannot be null or empty");

        if (!value.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
            throw new JsonException($"JMAP UTCDate must end with 'Z': {value}");

        return !DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal,
            out var dateTime)
            ? throw new JsonException($"Invalid JMAP UTCDate format: {value}")
            : new JmapUtcDate(dateTime);
    }

    /// <summary>
    /// Writes a <see cref="JmapUtcDate" /> as a JSON string value.
    /// </summary>
    /// <param name="writer">The writer to write JSON to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, JmapUtcDate value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
