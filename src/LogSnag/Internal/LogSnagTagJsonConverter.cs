using System.Text.Json;
using System.Text.Json.Serialization;

namespace LogSnag.Internal;

internal sealed class LogSnagTagJsonConverter : JsonConverter<LogSnagTag>
{
    public override LogSnagTag Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, LogSnagTag value, JsonSerializerOptions options)
    {
        switch (value.Value)
        {
            case int number:
                writer.WriteNumber(value.Key, number);
                break;
            case bool boolean:
                writer.WriteBoolean(value.Key, boolean);
                break;
            case string str when !string.IsNullOrWhiteSpace(str):
                writer.WriteString(value.Key, str);
                break;
            default:
                throw new JsonException(
                    $"{nameof(LogSnagTag)} was initialized without a valid value (string, boolean or integer)");
        }
    }
}