using ReformaTributaria.API.Utils.Enums;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReformaTributaria.API.Utils.Json.JsonConverter;

public class ETipoAliquotaConverter : JsonConverter<ETipoAliquota>
{
    public override bool CanConvert(Type t) => t == typeof(ETipoAliquota);

    public override ETipoAliquota Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        switch (value)
        {
            case "Fixa":
                return ETipoAliquota.Fixa;
            case "Padrão":
                return ETipoAliquota.Padrão;
            case "Sem alíquota":
                return ETipoAliquota.SemAlíquota;
            case "Uniforme nacional (referência)":
                return ETipoAliquota.UniformeNacionalReferência;
            case "Uniforme setorial":
                return ETipoAliquota.UniformeSetorial;
        }
        throw new Exception("Cannot unmarshal type TipoAliquota");
    }

    public override void Write(Utf8JsonWriter writer, ETipoAliquota value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ETipoAliquota.Fixa:
                JsonSerializer.Serialize(writer, "Fixa", options);
                return;
            case ETipoAliquota.Padrão:
                JsonSerializer.Serialize(writer, "Padrão", options);
                return;
            case ETipoAliquota.SemAlíquota:
                JsonSerializer.Serialize(writer, "Sem alíquota", options);
                return;
            case ETipoAliquota.UniformeNacionalReferência:
                JsonSerializer.Serialize(writer, "Uniforme nacional (referência)", options);
                return;
            case ETipoAliquota.UniformeSetorial:
                JsonSerializer.Serialize(writer, "Uniforme setorial", options);
                return;
        }
        throw new Exception("Cannot marshal type TipoAliquota");
    }

    public static readonly ETipoAliquotaConverter Singleton = new();
}
