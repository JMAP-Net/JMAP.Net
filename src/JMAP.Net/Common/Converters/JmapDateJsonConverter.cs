using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Common.Converters;

/// <summary>
/// JSON converter for JmapDate type.
/// </summary>
public class JmapDateJsonConverter : JsonConverter<JmapDate>
{
    public override JmapDate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            throw new JsonException("JMAP Date cannot be null or empty");

        return !DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime)
            ? throw new JsonException($"Invalid JMAP Date format: {value}")
            : new JmapDate(dateTime);
    }

    public override void Write(Utf8JsonWriter writer, JmapDate value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}