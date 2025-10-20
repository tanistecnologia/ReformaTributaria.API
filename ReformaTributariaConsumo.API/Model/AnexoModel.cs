using System.Text.Json.Serialization;

namespace ReformaTributaria.API.Model;

[JsonConverter(typeof(JsonStringEnumConverter<TipoCodigo>))]
public enum TipoCodigo
{
    NCM = 1,
    NBS = 2
}

public class AnexoModel
{
    public int Anexo { get; set; }
    
    public int Tipo { get; set; }
    public string Codigo { get; set; } = string.Empty;
    [JsonPropertyName("ini_vigencia")] public string IniVigencia { get; set; } = string.Empty;
    [JsonPropertyName("fim_vigencia")] public string FimVigencia { get; set; } = string.Empty;
}

public class AnexoListModel
{
    public int Anexo { get; set; }
    public TipoCodigo Tipo { get; set; }
    public string Codigo { get; set; } = string.Empty;
    [JsonPropertyName("ini_vigencia")] public DateTime? IniVigencia { get; set; }
    [JsonPropertyName("fim_vigencia")] public DateTime? FimVigencia { get; set; }
}