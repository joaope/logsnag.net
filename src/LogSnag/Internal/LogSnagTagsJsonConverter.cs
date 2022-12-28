using System.Text.Json;
using System.Text.Json.Serialization;

namespace LogSnag.Internal;

internal sealed class LogSnagTagsJsonConverter : JsonConverter<LogSnagTags>
{
    public override LogSnagTags Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, LogSnagTags value, JsonSerializerOptions options)
    {
        if (value.Count > 0)
        {
            writer.WriteStartObject();

            foreach (var tag in value)
            {
                JsonSerializer.Serialize(writer, tag, options);
            }

            writer.WriteEndObject();
        }
    }
}