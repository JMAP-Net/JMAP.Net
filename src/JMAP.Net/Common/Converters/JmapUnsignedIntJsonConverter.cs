using System.Text.Json;
using System.Text.Json.Serialization;
using JMAP.Net.Capabilities.Core.Types;

namespace JMAP.Net.Common.Converters;

/// <summary>
/// JSON converter for JmapUnsignedInt type.
/// </summary>
public class JmapUnsignedIntJsonConverter : JsonConverter<JmapUnsignedInt>
{
    public override JmapUnsignedInt Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException("Expected number token for JmapUnsignedInt");
        
        var value = reader.GetInt64();
        return new JmapUnsignedInt(value);
    }

    public override void Write(Utf8JsonWriter writer, JmapUnsignedInt value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}