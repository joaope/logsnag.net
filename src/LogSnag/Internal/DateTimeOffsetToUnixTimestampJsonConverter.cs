using System.Text.Json;
using System.Text.Json.Serialization;

namespace LogSnag.Internal;

internal sealed class DateTimeOffsetToUnixTimestampJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToUnixTimeMilliseconds().ToString());
    }
}