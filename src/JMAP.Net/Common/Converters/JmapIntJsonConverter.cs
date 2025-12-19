using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Common.Converters;

/// <summary>
/// JSON converter for JmapInt type.
/// </summary>
public class JmapIntJsonConverter : JsonConverter<JmapInt>
{
    public override JmapInt Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException("Expected number token for JmapInt");

        var value = reader.GetInt64();
        return new JmapInt(value);
    }

    public override void Write(Utf8JsonWriter writer, JmapInt value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}