using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Common.Converters;

/// <summary>
/// JSON converter for JmapId type.
/// </summary>
public class JmapIdJsonConverter : JsonConverter<JmapId>
{
    public override JmapId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrEmpty(value) 
            ? throw new JsonException("JMAP Id cannot be null or empty")
            : new JmapId(value);
    }

    public override void Write(Utf8JsonWriter writer, JmapId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}