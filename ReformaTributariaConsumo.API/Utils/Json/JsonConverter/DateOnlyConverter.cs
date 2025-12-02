using Microsoft.IdentityModel.Tokens;

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReformaTributaria.API.Utils.Json.JsonConverter;

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    private readonly string serializationFormat = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        //var value = reader.GetString();
        //return DateOnly.Parse(value!);
        var strDate = reader.GetString();

        if (strDate == null || strDate.IsNullOrEmpty()) return DateOnly.MinValue;

        return DateOnly.ParseExact(strDate, serializationFormat, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        //writer.WriteStringValue(value.ToString(serializationFormat));

        writer.WriteStringValue(value.ToString(serializationFormat, CultureInfo.InvariantCulture));
    }
}


public class DateOnlyNullableConverter : JsonConverter<DateOnly?>
{
    private readonly string serializationFormat = "yyyy-MM-dd";

    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        //var value = reader.GetString();
        //return DateOnly.Parse(value!);
        var strDate = reader.GetString();
        if (strDate == null || strDate.IsNullOrEmpty()) return null;
        return DateOnly.ParseExact(strDate, serializationFormat, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        //writer.WriteStringValue(value.ToString(serializationFormat));

        if (value == null)
        {
            writer.WriteStringValue("");
            return;
        }

        DateOnly data = value.Value;
        writer.WriteStringValue(data.ToString(serializationFormat, CultureInfo.InvariantCulture));
    }
}