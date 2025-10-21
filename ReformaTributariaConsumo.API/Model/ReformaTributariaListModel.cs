namespace ReformaTributaria.API.Model;

public class ReformaTributariaListModel
{
    public string COD_CST { get; set; } = string.Empty;
    public string DS_SIT_TRIBUTARIA { get; set; } = string.Empty;
    public string EXIGE_TRIB { get; set; } = string.Empty;
    public string RED_BC_CST { get; set; } = string.Empty;
    public string RED_ALIQUOTA { get; set; } = string.Empty;
    public string TRANSF_CREDITO { get; set; } = string.Empty;
    public string DIFERIMENTO { get; set; } = string.Empty;
    public string MONOFASICA { get; set; } = string.Empty;
    public string COD_CLASS_TRIB { get; set; } = string.Empty;
    public string DS_COD_CLASS_TRIB { get; set; } = string.Empty;
    public string NOME_CLASS_TRIB { get; set; } = string.Empty;
    public string REDACAO_LC_214_2025 { get; set; } = string.Empty;
    public string ARTIGO_LC_214_2025 { get; set; } = string.Empty;
    public string TIPO_ALIQUOTA { get; set; } = string.Empty;
    public decimal PERC_RED_IBS { get; set; }
    public decimal PERC_RED_CBS { get; set; }
    public string RED_BC { get; set; } = string.Empty;
    public string TRIB_REGULAR { get; set; } = string.Empty;
    public string CRED_PRESUMIDO { get; set; } = string.Empty;
    public string CRED_PARA { get; set; } = string.Empty;
    public string IND_RED_BC { get; set; } = string.Empty;
    public string IND_MONO_PADRAO { get; set; } = string.Empty;
    public string IND_MONO_RETENCAO { get; set; } = string.Empty;
    public string IND_MONO_RETIDO { get; set; } = string.Empty;
    public string IND_MONO_DIFERIMENTO { get; set; } = string.Empty;
    public string IND_ESTORNO_CREDITO { get; set; } = string.Empty;
    public string IND_CRED_PRESUMIDO_ZFM { get; set; } = string.Empty;
    public string IND_AJUSTE_COMPETENCIA { get; set; } = string.Empty;
    public DateTime? DT_INI_VIGENCIA { get; set; }
    public DateTime? DT_FIM_VIGENCIA { get; set; }
    public DateTime? DT_ATUALIZACAO { get; set; }
    public string ANEXO { get; set; } = string.Empty;
    public string LINK_LEGISLACAO { get; set; } = string.Empty;
    public string APLICA_NFE_ABI { get; set; } = string.Empty;
    public string APLICA_NFE { get; set; } = string.Empty;
    public string APLICA_NFCE { get; set; } = string.Empty;
    public string APLICA_CTE { get; set; } = string.Empty;
    public string APLICA_CTE_OS { get; set; } = string.Empty;
    public string APLICA_BPE { get; set; } = string.Empty;
    public string APLICA_BPE_TA { get; set; } = string.Empty;
    public string APLICA_BPE_TM { get; set; } = string.Empty;
    public string APLICA_NF3E { get; set; } = string.Empty;
    public string APLICA_NFSE { get; set; } = string.Empty;
    public string APLICA_NFSE_VIA { get; set; } = string.Empty;
    public string APLICA_NFCOM { get; set; } = string.Empty;
    public string APLICA_NFAG { get; set; } = string.Empty;
    public string APLICA_NFGAS { get; set; } = string.Empty;
    public string APLICA_DERE { get; set; } = string.Empty;
}