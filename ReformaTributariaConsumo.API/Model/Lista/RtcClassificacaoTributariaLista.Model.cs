using ReformaTributaria.API.Utils.Enums;

using System.Text.Json.Serialization;

namespace ReformaTributaria.API.Model.Lista;

public class RtcSituacaoTributariaListaModel
{
    [JsonPropertyName("RST_COD_CST")]
    public string RST_COD_CST { get; set; } = string.Empty;

    [JsonPropertyName("RST_DS_SITUACAO_TRIBUTARIA")]
    public string RST_DS_SITUACAO_TRIBUTARIA { get; set; } = string.Empty;

    [JsonPropertyName("RST_IND_GIBSCBS")]
    public ESN RST_IND_GIBSCBS { get; set; }

    [JsonPropertyName("RST_IND_GIBSCBSMONO")]
    public ESN RST_IND_GIBSCBSMONO { get; set; }

    [JsonPropertyName("RST_IND_GRED")]
    public ESN RST_IND_GRED { get; set; }

    [JsonPropertyName("RST_IND_GDIF")]
    public ESN RST_IND_GDIF { get; set; }

    [JsonPropertyName("RST_IND_GTRANSFCRED")]
    public ESN RST_IND_GTRANSFCRED { get; set; }

    [JsonPropertyName("RST_IND_GCREDPRESIBSZFM")]
    public ESN RST_IND_GCREDPRESIBSZFM { get; set; }

    [JsonPropertyName("RST_IND_REDUTORBC")]
    public ESN RST_IND_REDUTORBC { get; set; }

    [JsonPropertyName("RST_DT_ATUALIZACAO")]
    public DateTime RST_DT_ATUALIZACAO { get; set; }
}

public class RtcClassificacaoTributariaListaModel
{
    [JsonPropertyName("RCT_COD_CLASS_TRIB")]
    public string RCT_COD_CLASS_TRIB { get; set; } = string.Empty;

    [JsonPropertyName("CST")]
    public RtcSituacaoTributariaListaModel CST { get; set; } = new();

    [JsonPropertyName("RTA_COD_ANEXO")]
    public string RTA_COD_ANEXO { get; set; } = string.Empty;

    [JsonPropertyName("RCT_DS_CLASS_TRIB")]
    public string RCT_DS_CLASS_TRIB { get; set; } = string.Empty;

    [JsonPropertyName("RCT_NOME_CLASS_TRIB")]
    public string RCT_NOME_CLASS_TRIB { get; set; } = string.Empty;

    [JsonPropertyName("RCT_LC_REDACAO")]
    public string RCT_LC_REDACAO { get; set; } = string.Empty;

    [JsonPropertyName("RCT_LC_214_25")]
    public string RCT_LC_214_25 { get; set; } = string.Empty;

    [JsonPropertyName("RCT_TIPO_ALIQUOTA")]
    public string RCT_TIPO_ALIQUOTA { get; set; } = string.Empty;

    [JsonPropertyName("RCT_PERC_RED_IBS")]
    public decimal RCT_PERC_RED_IBS { get; set; }

    [JsonPropertyName("RCT_PERC_RED_CBS")]
    public decimal RCT_PERC_RED_CBS { get; set; }

    [JsonPropertyName("RCT_IND_GTRIBREGULAR")]
    public ESN RCT_IND_GTRIBREGULAR { get; set; }

    [JsonPropertyName("RCT_IND_GCREDPRESOPER")]
    public ESN RCT_IND_GCREDPRESOPER { get; set; }

    [JsonPropertyName("RCT_IND_GMONOPADRAO")]
    public ESN RCT_IND_GMONOPADRAO { get; set; }

    [JsonPropertyName("RCT_IND_GMONORETEN")]
    public ESN RCT_IND_GMONORETEN { get; set; }

    [JsonPropertyName("RCT_IND_GMONORET")]
    public ESN RCT_IND_GMONORET { get; set; }

    [JsonPropertyName("RCT_IND_GMONODIF")]
    public ESN RCT_IND_GMONODIF { get; set; }

    [JsonPropertyName("RCT_IND_GESTORNOCRED")]
    public ESN RCT_IND_GESTORNOCRED { get; set; }

    [JsonPropertyName("RCT_IND_NFEABI")]
    public ESN RCT_IND_NFEABI { get; set; }

    [JsonPropertyName("RCT_IND_NFE")]
    public ESN RCT_IND_NFE { get; set; }

    [JsonPropertyName("RCT_IND_NFCE")]
    public ESN RCT_IND_NFCE { get; set; }

    [JsonPropertyName("RCT_IND_CTE")]
    public ESN RCT_IND_CTE { get; set; }

    [JsonPropertyName("RCT_IND_CTEOS")]
    public ESN RCT_IND_CTEOS { get; set; }

    [JsonPropertyName("RCT_IND_BPE")]
    public ESN RCT_IND_BPE { get; set; }

    [JsonPropertyName("RCT_IND_BPETA")]
    public ESN RCT_IND_BPETA { get; set; }

    [JsonPropertyName("RCT_IND_BPETM")]
    public ESN RCT_IND_BPETM { get; set; }

    [JsonPropertyName("RCT_IND_NF3E")]
    public ESN RCT_IND_NF3E { get; set; }

    [JsonPropertyName("RCT_IND_NFSE")]
    public ESN RCT_IND_NFSE { get; set; }

    [JsonPropertyName("RCT_IND_NFSEVIA")]
    public ESN RCT_IND_NFSEVIA { get; set; }

    [JsonPropertyName("RCT_IND_NFCOM")]
    public ESN RCT_IND_NFCOM { get; set; }

    [JsonPropertyName("RCT_IND_NFAG")]
    public ESN RCT_IND_NFAG { get; set; }

    [JsonPropertyName("RCT_IND_NFGAS")]
    public ESN RCT_IND_NFGAS { get; set; }

    [JsonPropertyName("RCT_IND_DERE")]
    public ESN RCT_IND_DERE { get; set; }

    [JsonPropertyName("RCT_LINK_LEGISLACAO")]
    public string RCT_LINK_LEGISLACAO { get; set; } = string.Empty;

    [JsonPropertyName("RCT_DT_INI_VIGENCIA")]
    //[JsonConverter(typeof(DateOnlyConverter))]
    public DateTime? RCT_DT_INI_VIGENCIA { get; set; }

    [JsonPropertyName("RCT_DT_FIM_VIGENCIA")]
    //[JsonConverter(typeof(DateOnlyNullableConverter))]
    public DateTime? RCT_DT_FIM_VIGENCIA { get; set; }

    [JsonPropertyName("RCT_DT_ATUALIZACAO")]
    public DateTime RCT_DT_ATUALIZACAO { get; set; }
}



