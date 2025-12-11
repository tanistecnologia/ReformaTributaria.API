using ReformaTributaria.API.Utils.Enums;

using System.Text.Json.Serialization;

namespace ReformaTributaria.API.Model.Lista;

public class RtcSituacaoTributariaListaModel
{
    [JsonPropertyName("COD_CST")]
    public string RST_COD_CST { get; set; } = string.Empty;

    [JsonPropertyName("DS_SITUACAO_TRIBUTARIA")]
    public string RST_DS_SITUACAO_TRIBUTARIA { get; set; } = string.Empty;

    [JsonPropertyName("IND_GIBSCBS")]
    public ESN RST_IND_GIBSCBS { get; set; }

    [JsonPropertyName("IND_GIBSCBSMONO")]
    public ESN RST_IND_GIBSCBSMONO { get; set; }

    [JsonPropertyName("IND_GRED")]
    public ESN RST_IND_GRED { get; set; }

    [JsonPropertyName("IND_GDIF")]
    public ESN RST_IND_GDIF { get; set; }

    [JsonPropertyName("IND_GTRANSFCRED")]
    public ESN RST_IND_GTRANSFCRED { get; set; }

    [JsonPropertyName("IND_GCREDPRESIBSZFM")]
    public ESN RST_IND_GCREDPRESIBSZFM { get; set; }

    [JsonPropertyName("IND_REDUTORBC")]
    public ESN RST_IND_REDUTORBC { get; set; }

    [JsonPropertyName("DT_ATUALIZACAO")]
    public DateTime RST_DT_ATUALIZACAO { get; set; }
}

public class RtcClassificacaoTributariaListaModel
{
    [JsonPropertyName("COD_CLASS_TRIB")]
    public string RCT_COD_CLASS_TRIB { get; set; } = string.Empty;

    [JsonPropertyName("CST")]
    public RtcSituacaoTributariaListaModel CST { get; set; } = new();

    [JsonPropertyName("COD_ANEXO")]
    public string RTA_COD_ANEXO { get; set; } = string.Empty;

    [JsonPropertyName("DS_CLASS_TRIB")]
    public string RCT_DS_CLASS_TRIB { get; set; } = string.Empty;

    [JsonPropertyName("NOME_CLASS_TRIB")]
    public string RCT_NOME_CLASS_TRIB { get; set; } = string.Empty;

    [JsonPropertyName("LC_REDACAO")]
    public string RCT_LC_REDACAO { get; set; } = string.Empty;

    [JsonPropertyName("LC_214_25")]
    public string RCT_LC_214_25 { get; set; } = string.Empty;

    [JsonPropertyName("TIPO_ALIQUOTA")]
    public string RCT_TIPO_ALIQUOTA { get; set; } = string.Empty;

    [JsonPropertyName("PERC_RED_IBS")]
    public decimal RCT_PERC_RED_IBS { get; set; }

    [JsonPropertyName("PERC_RED_CBS")]
    public decimal RCT_PERC_RED_CBS { get; set; }

    [JsonPropertyName("IND_GTRIBREGULAR")]
    public ESN RCT_IND_GTRIBREGULAR { get; set; }

    [JsonPropertyName("IND_GCREDPRESOPER")]
    public ESN RCT_IND_GCREDPRESOPER { get; set; }

    [JsonPropertyName("IND_GMONOPADRAO")]
    public ESN RCT_IND_GMONOPADRAO { get; set; }

    [JsonPropertyName("IND_GMONORETEN")]
    public ESN RCT_IND_GMONORETEN { get; set; }

    [JsonPropertyName("IND_GMONORET")]
    public ESN RCT_IND_GMONORET { get; set; }

    [JsonPropertyName("IND_GMONODIF")]
    public ESN RCT_IND_GMONODIF { get; set; }

    [JsonPropertyName("IND_GESTORNOCRED")]
    public ESN RCT_IND_GESTORNOCRED { get; set; }

    [JsonPropertyName("IND_NFEABI")]
    public ESN RCT_IND_NFEABI { get; set; }

    [JsonPropertyName("IND_NFE")]
    public ESN RCT_IND_NFE { get; set; }

    [JsonPropertyName("IND_NFCE")]
    public ESN RCT_IND_NFCE { get; set; }

    [JsonPropertyName("IND_CTE")]
    public ESN RCT_IND_CTE { get; set; }

    [JsonPropertyName("IND_CTEOS")]
    public ESN RCT_IND_CTEOS { get; set; }

    [JsonPropertyName("IND_BPE")]
    public ESN RCT_IND_BPE { get; set; }

    [JsonPropertyName("IND_BPETA")]
    public ESN RCT_IND_BPETA { get; set; }

    [JsonPropertyName("IND_BPETM")]
    public ESN RCT_IND_BPETM { get; set; }

    [JsonPropertyName("IND_NF3E")]
    public ESN RCT_IND_NF3E { get; set; }

    [JsonPropertyName("IND_NFSE")]
    public ESN RCT_IND_NFSE { get; set; }

    [JsonPropertyName("IND_NFSEVIA")]
    public ESN RCT_IND_NFSEVIA { get; set; }

    [JsonPropertyName("IND_NFCOM")]
    public ESN RCT_IND_NFCOM { get; set; }

    [JsonPropertyName("IND_NFAG")]
    public ESN RCT_IND_NFAG { get; set; }

    [JsonPropertyName("IND_NFGAS")]
    public ESN RCT_IND_NFGAS { get; set; }

    [JsonPropertyName("IND_DERE")]
    public ESN RCT_IND_DERE { get; set; }

    [JsonPropertyName("LINK_LEGISLACAO")]
    public string RCT_LINK_LEGISLACAO { get; set; } = string.Empty;

    [JsonPropertyName("DT_INI_VIGENCIA")]
    //[JsonConverter(typeof(DateOnlyConverter))]
    public DateTime? RCT_DT_INI_VIGENCIA { get; set; }

    [JsonPropertyName("DT_FIM_VIGENCIA")]
    //[JsonConverter(typeof(DateOnlyNullableConverter))]
    public DateTime? RCT_DT_FIM_VIGENCIA { get; set; }

    [JsonPropertyName("DT_ATUALIZACAO")]
    public DateTime RCT_DT_ATUALIZACAO { get; set; }
}



