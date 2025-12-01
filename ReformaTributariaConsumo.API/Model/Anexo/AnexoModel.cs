using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReformaTributaria.API.Model.Anexo;

[JsonConverter(typeof(ETipoCodigoConverter))]
public enum ETipoCodigo
{
    NCM = 1,
    NBS = 2
}

public class AnexoModel
{
    public int Anexo { get; set; }

    public ETipoCodigo Tipo { get; set; }
    public string Codigo { get; set; } = string.Empty;
    [JsonPropertyName("ini_vigencia")] public string IniVigencia { get; set; } = string.Empty;
    [JsonPropertyName("fim_vigencia")] public string FimVigencia { get; set; } = string.Empty;
}

public class AnexoListModel
{
    public int Anexo { get; set; }
    public ETipoCodigo Tipo { get; set; }
    public string Codigo { get; set; } = string.Empty;
    [JsonPropertyName("ini_vigencia")]
    public DateTime? IniVigencia { get; set; }
    [JsonPropertyName("fim_vigencia")]
    public DateTime? FimVigencia { get; set; }
    [JsonPropertyName("atualizado_em")]
    public DateTime? AtualizadoEm { get; set; }
}

internal class ETipoCodigoConverter : JsonConverter<ETipoCodigo>
{
    public override bool CanConvert(Type t) => t == typeof(ETipoCodigo);

    public override ETipoCodigo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value;
        if (reader.TokenType == JsonTokenType.Number)
            value = reader.GetInt16().ToString();
        else
            value = reader.GetString() ?? "";

        return value switch
        {
            "NCM" or "1" => ETipoCodigo.NCM,
            "NBS" or "2" => ETipoCodigo.NBS,
            _ => throw new Exception("Cannot unmarshal type TipoAliquota")
        };
    }

    public override void Write(Utf8JsonWriter writer, ETipoCodigo value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ETipoCodigo.NCM:
                JsonSerializer.Serialize(writer, "NCM", options);
                return;
            case ETipoCodigo.NBS:
                JsonSerializer.Serialize(writer, "NBS", options);
                return;
        }
        throw new Exception("Cannot marshal type TipoAliquota");
    }

    public static readonly ETipoCodigoConverter Singleton = new();
}