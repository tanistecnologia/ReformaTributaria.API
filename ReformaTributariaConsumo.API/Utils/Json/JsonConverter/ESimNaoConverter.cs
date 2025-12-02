using ReformaTributaria.API.Utils.Enums;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReformaTributaria.API.Utils.Json.JsonConverter;

public class ESimNaoConverter : JsonConverter<ESimNao>
{
    //public override bool CanConvert(Type t) => t == typeof(ESimNao);

    public override ESimNao Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        switch (value)
        {
            case "Não":
                return ESimNao.Não;
            case "N":
                return ESimNao.Não;
            case "Sim":
                return ESimNao.Sim;
            case "S":
                return ESimNao.Sim;
            default:
                break;
        }
        throw new Exception("Cannot unmarshal type Ind");
    }

    public override void Write(Utf8JsonWriter writer, ESimNao value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ESimNao.Não:
                JsonSerializer.Serialize(writer, "Não", options);
                return;
            case ESimNao.Sim:
                JsonSerializer.Serialize(writer, "Sim", options);
                return;
        }
        throw new Exception("Cannot marshal type Ind");
    }

    public static readonly ESimNaoConverter Singleton = new();
}

public class ESNConverter : JsonConverter<ESN>
{
    //public override bool CanConvert(Type t) => t == typeof(ESimNao);

    public override ESN Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        switch (value)
        {
            case "Não":
                return ESN.N;
            case "N":
                return ESN.N;
            case "Sim":
                return ESN.S;
            case "S":
                return ESN.S;
            default:
                break;
        }
        throw new Exception("Cannot unmarshal type Ind");
    }

    public override void Write(Utf8JsonWriter writer, ESN value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ESN.N:
                JsonSerializer.Serialize(writer, "N", options);
                return;
            case ESN.S:
                JsonSerializer.Serialize(writer, "S", options);
                return;
        }
        throw new Exception("Cannot marshal type Ind");
    }

    public static readonly ESimNaoConverter Singleton = new();
}
