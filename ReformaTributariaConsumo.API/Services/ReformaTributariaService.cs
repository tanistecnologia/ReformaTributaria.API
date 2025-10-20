using System.Data;
using Dapper;
using Dapper.Transaction;
using ReformaTributaria.API.Model;
using ReformaTributaria.API.Utils;
using TClassificacaoTributaria;

namespace ReformaTributaria.API.Services;

public class ReformaTributariaService(ILogger<ReformaTributariaService> logger, IDbConnection connection)
{
    public async Task<string> InsereDados(List<ClassificacaoTributariaModel> listClassificacaoTributaria)
    {
        var rowsAffected = 0;
        
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                await transaction.ExecuteAsync("delete from RFC.TBL_CLASSIFICACAO_TRIBUTARIA");

                foreach (var classificacaoTributaria in listClassificacaoTributaria)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("COD_CST", classificacaoTributaria.CódigoDaSituaçãoTributária,
                        dbType: DbType.String);
                    parameters.Add("DS_SIT_TRIBUTARIA", classificacaoTributaria.DescriçãoDoCódigoDaSituaçãoTributária,
                        dbType: DbType.String);
                    parameters.Add("EXIGE_TRIB", classificacaoTributaria.ExigeTributação.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("RED_BC_CST", classificacaoTributaria.ReduçãoBcCst.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("RED_ALIQUOTA", classificacaoTributaria.ReduçãoAlíquota.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("TRANSF_CREDITO", classificacaoTributaria.TransferênciaCrédito.SafeSubstring(0, 1),
                        dbType: DbType.String);
                    parameters.Add("DIFERIMENTO", classificacaoTributaria.Diferimento.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("MONOFASICA", classificacaoTributaria.Monofásica.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("COD_CLASS_TRIB", classificacaoTributaria.CódigoDaClassificaçãoTributária.Trim(),
                        dbType: DbType.String);
                    parameters.Add("DS_COD_CLASS_TRIB",
                        classificacaoTributaria.DescriçãoDoCódigoDaClassificaçãoTributária, dbType: DbType.String);
                    parameters.Add("NOME_CLASS_TRIB", classificacaoTributaria.NomeDaClassificaçãoTributária,
                        dbType: DbType.String);
                    parameters.Add("REDACAO_LC_214_2025", classificacaoTributaria.RedaçãoLc2142025,
                        dbType: DbType.String);
                    parameters.Add("ARTIGO_LC_214_2025", classificacaoTributaria.ArtigoLc2142025,
                        dbType: DbType.String);
                    parameters.Add("TIPO_ALIQUOTA", classificacaoTributaria.TipoDeAlíquota, dbType: DbType.String);
                    parameters.Add("PERC_RED_IBS", classificacaoTributaria.PercentualReduçãoIbs,
                        dbType: DbType.Decimal);
                    parameters.Add("PERC_RED_CBS", classificacaoTributaria.PercentualReduçãoCbs,
                        dbType: DbType.Decimal);
                    parameters.Add("RED_BC", classificacaoTributaria.ReduçãoBc.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("TRIB_REGULAR", classificacaoTributaria.TributaçãoRegular.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("CRED_PRESUMIDO", classificacaoTributaria.CréditoPresumido.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("CRED_PARA", classificacaoTributaria.CréditoPara, dbType: DbType.String);
                    parameters.Add("IND_RED_BC", classificacaoTributaria.IndicadorRedutorBc.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("IND_MONO_PADRAO", classificacaoTributaria.IndicadorMonofásicaPadrão.SafeSubstring(0, 1),
                        dbType: DbType.String);
                    parameters.Add("IND_MONO_RETENCAO", classificacaoTributaria.IndicadorMonofásicaRetenção.SafeSubstring(0, 1),
                        dbType: DbType.String);
                    parameters.Add("IND_MONO_RETIDO", classificacaoTributaria.IndicadorMonofásicaRetido.SafeSubstring(0, 1),
                        dbType: DbType.String);
                    parameters.Add("IND_MONO_DIFERIMENTO", classificacaoTributaria.IndicadorMonofásicaDiferimento.SafeSubstring(0, 1),
                        dbType: DbType.String);
                    parameters.Add("IND_ESTORNO_CREDITO", classificacaoTributaria.IndicadorEstornoCrédito.SafeSubstring(0, 1),
                        dbType: DbType.String);
                    parameters.Add("IND_CRED_PRESUMIDO_ZFM", classificacaoTributaria.IndicadorCréditoPresumidoZfm.SafeSubstring(0, 1),
                        dbType: DbType.String);
                    parameters.Add("IND_AJUSTE_COMPETENCIA", classificacaoTributaria.IndicadorAjusteCompetência.SafeSubstring(0, 1),
                        dbType: DbType.String);

                    if (classificacaoTributaria.DataInícioVigência != string.Empty)
                    {
                        var date = DateOnly.ParseExact(classificacaoTributaria.DataInícioVigência, "yyyy-MM-dd");
                        parameters.Add("DT_INI_VIGENCIA", date, dbType: DbType.Date);
                    }
                    else
                    {
                        parameters.Add("DT_INI_VIGENCIA", DBNull.Value, dbType: DbType.Date);
                    }
                    
                    if (classificacaoTributaria.DataFimVigência != string.Empty)
                    {
                        var date = DateOnly.ParseExact(classificacaoTributaria.DataFimVigência, "yyyy-MM-dd");
                        parameters.Add("DT_FIM_VIGENCIA", date, dbType: DbType.Date);
                    }
                    else
                    {
                        parameters.Add("DT_FIM_VIGENCIA", DBNull.Value, dbType: DbType.Date);
                    }                    
                    
                    if (classificacaoTributaria.DataAtualização != string.Empty)
                    {
                        var date = DateOnly.ParseExact(classificacaoTributaria.DataAtualização, "yyyy-MM-dd");
                        parameters.Add("DT_ATUALIZACAO", date, dbType: DbType.Date);
                    }
                    else
                    {
                        parameters.Add("DT_ATUALIZACAO", DBNull.Value, dbType: DbType.Date);
                    }
                    
                    parameters.Add("ANEXO", classificacaoTributaria.Anexo, dbType: DbType.String);
                    parameters.Add("LINK_LEGISLACAO", classificacaoTributaria.LinkLegislação, dbType: DbType.String);
                    parameters.Add("APLICA_NFE_ABI", classificacaoTributaria.AplicaNFeAbi.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFE", classificacaoTributaria.AplicaNFe.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFCE", classificacaoTributaria.AplicaNfCe.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_CTE", classificacaoTributaria.AplicaCTe.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_CTE_OS", classificacaoTributaria.AplicaCTeOs.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_BPE", classificacaoTributaria.AplicaBPe.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_BPE_TA", classificacaoTributaria.AplicaBPeTransporteAéreo.SafeSubstring(0, 1),
                        dbType: DbType.String);
                    parameters.Add("APLICA_BPE_TM", classificacaoTributaria.AplicaBPeTransporteMarítimo.SafeSubstring(0, 1),
                        dbType: DbType.String);
                    parameters.Add("APLICA_NF3E", classificacaoTributaria.AplicaNf3E.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFSE", classificacaoTributaria.AplicaNfSe.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFSE_VIA", classificacaoTributaria.AplicaNfSeVia.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFCOM", classificacaoTributaria.AplicaNfCom.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFAG", classificacaoTributaria.AplicaNfAg.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFGAS", classificacaoTributaria.AplicaNfGas.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_DERE", classificacaoTributaria.AplicaDere.SafeSubstring(0, 1), dbType: DbType.String);

                    rowsAffected += await transaction.ExecuteAsync(
                        sql: @"
							INSERT INTO RFC.TBL_CLASSIFICACAO_TRIBUTARIA
								(COD_CST, DS_SIT_TRIBUTARIA, EXIGE_TRIB, RED_BC_CST, RED_ALIQUOTA, TRANSF_CREDITO, DIFERIMENTO, MONOFASICA, COD_CLASS_TRIB, 
								 DS_COD_CLASS_TRIB, NOME_CLASS_TRIB, REDACAO_LC_214_2025, ARTIGO_LC_214_2025, TIPO_ALIQUOTA, PERC_RED_IBS, PERC_RED_CBS, 
								 RED_BC, TRIB_REGULAR, CRED_PRESUMIDO, CRED_PARA, IND_RED_BC, IND_MONO_PADRAO, IND_MONO_RETENCAO, IND_MONO_RETIDO, 
								 IND_MONO_DIFERIMENTO, IND_ESTORNO_CREDITO, IND_CRED_PRESUMIDO_ZFM, IND_AJUSTE_COMPETENCIA, DT_INI_VIGENCIA, DT_FIM_VIGENCIA, 
								 DT_ATUALIZACAO, ANEXO, LINK_LEGISLACAO, APLICA_NFE_ABI, APLICA_NFE, APLICA_NFCE, APLICA_CTE, APLICA_CTE_OS, APLICA_BPE, 
								 APLICA_BPE_TA, APLICA_BPE_TM, APLICA_NF3E, APLICA_NFSE, APLICA_NFSE_VIA, APLICA_NFCOM, APLICA_NFAG, APLICA_NFGAS, APLICA_DERE)
							VALUES
								(@COD_CST, @DS_SIT_TRIBUTARIA, @EXIGE_TRIB, @RED_BC_CST, @RED_ALIQUOTA, @TRANSF_CREDITO, @DIFERIMENTO, @MONOFASICA, @COD_CLASS_TRIB, 
								 @DS_COD_CLASS_TRIB, @NOME_CLASS_TRIB, @REDACAO_LC_214_2025, @ARTIGO_LC_214_2025, @TIPO_ALIQUOTA, @PERC_RED_IBS, @PERC_RED_CBS, 
								 @RED_BC, @TRIB_REGULAR, @CRED_PRESUMIDO, @CRED_PARA, @IND_RED_BC, @IND_MONO_PADRAO, @IND_MONO_RETENCAO, @IND_MONO_RETIDO, 
								 @IND_MONO_DIFERIMENTO, @IND_ESTORNO_CREDITO, @IND_CRED_PRESUMIDO_ZFM, @IND_AJUSTE_COMPETENCIA, @DT_INI_VIGENCIA, @DT_FIM_VIGENCIA, 
								 @DT_ATUALIZACAO, @ANEXO, @LINK_LEGISLACAO, @APLICA_NFE_ABI, @APLICA_NFE, @APLICA_NFCE, @APLICA_CTE, @APLICA_CTE_OS, @APLICA_BPE, 
								 @APLICA_BPE_TA, @APLICA_BPE_TM, @APLICA_NF3E, @APLICA_NFSE, @APLICA_NFSE_VIA, @APLICA_NFCOM, @APLICA_NFAG, @APLICA_NFGAS, @APLICA_DERE)",
                        param: parameters
                    );

                    var logTxt =
                        $"Registro inserido: {classificacaoTributaria.CódigoDaClassificaçãoTributária.Trim()}/{rowsAffected}";
                    logger.LogInformation(logTxt);
                }

                transaction.Commit();
                return "ok";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                transaction.Rollback();
                return "Erro: " + e.Message;
            }
        }
    }

    public async Task<List<ReformaTributariaListModel>> GetDados()
    {
        var dados = await connection.QueryAsync<ReformaTributariaListModel>(
            sql: @"
                select 
                  COD_CST, DS_SIT_TRIBUTARIA, EXIGE_TRIB, RED_BC_CST, RED_ALIQUOTA, TRANSF_CREDITO, DIFERIMENTO, MONOFASICA, COD_CLASS_TRIB, 
                  DS_COD_CLASS_TRIB, NOME_CLASS_TRIB, REDACAO_LC_214_2025, ARTIGO_LC_214_2025, TIPO_ALIQUOTA, PERC_RED_IBS, PERC_RED_CBS, 
                  RED_BC, TRIB_REGULAR, CRED_PRESUMIDO, CRED_PARA, IND_RED_BC, IND_MONO_PADRAO, IND_MONO_RETENCAO, IND_MONO_RETIDO, 
                  IND_MONO_DIFERIMENTO, IND_ESTORNO_CREDITO, IND_CRED_PRESUMIDO_ZFM, IND_AJUSTE_COMPETENCIA, DT_INI_VIGENCIA, DT_FIM_VIGENCIA, 
                  DT_ATUALIZACAO, ANEXO, LINK_LEGISLACAO, APLICA_NFE_ABI, APLICA_NFE, APLICA_NFCE, APLICA_CTE, APLICA_CTE_OS, APLICA_BPE, 
				  APLICA_BPE_TA, APLICA_BPE_TM, APLICA_NF3E, APLICA_NFSE, APLICA_NFSE_VIA, APLICA_NFCOM, APLICA_NFAG, APLICA_NFGAS, APLICA_DERE 
                from
                  RFC.TBL_CLASSIFICACAO_TRIBUTARIA");

        return dados.ToList();
    }
    
    public async Task<string> InsereDadosAnexos(List<AnexoModel> anexos)
    {
        var rowsAffected = 0;
        
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                await transaction.ExecuteAsync("delete from RFC.TBL_CLASSIFICACAO_TRIBUTARIA_ANEXOS");

                foreach (var anexo in anexos)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("ANEXO", anexo.Anexo, dbType: DbType.Int64);
                    parameters.Add("TIPO_DOC", anexo.Tipo, dbType: DbType.Int16);
                    parameters.Add("CODIGO", anexo.Codigo, dbType: DbType.String);

                    if (anexo.IniVigencia != string.Empty)
                    {
                        var date = DateOnly.ParseExact(anexo.IniVigencia, "yyyy-MM-dd");
                        parameters.Add("DT_INICIO_VIGENCIA", date, dbType: DbType.Date);
                    }
                    else
                    {
                        parameters.Add("DT_INICIO_VIGENCIA", DBNull.Value, dbType: DbType.Date);
                    }
                    
                    if (anexo.FimVigencia != string.Empty)
                    {
                        var date = DateOnly.ParseExact(anexo.FimVigencia, "yyyy-MM-dd");
                        parameters.Add("DT_FIM_VIGENCIA", date, dbType: DbType.Date);
                    }
                    else
                    {
                        parameters.Add("DT_FIM_VIGENCIA", DBNull.Value, dbType: DbType.Date);
                    }                    

                    rowsAffected += await transaction.ExecuteAsync(
                        sql: @"
							INSERT INTO RFC.TBL_CLASSIFICACAO_TRIBUTARIA_ANEXOS
							  (ANEXO, TIPO_DOC, CODIGO, DT_INICIO_VIGENCIA, DT_FIM_VIGENCIA)
							VALUES
							  (@ANEXO, @TIPO_DOC, @CODIGO, @DT_INICIO_VIGENCIA, @DT_FIM_VIGENCIA)",
                        param: parameters
                    );

                    var logTxt =
                        $"Registro inserido: {anexo.Anexo} - {anexo.Tipo.ToString()} - {anexo.Codigo}/{rowsAffected}";
                    logger.LogInformation(logTxt);
                }

                transaction.Commit();
                return "ok";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                transaction.Rollback();
                return "Erro: " + e.Message;
            }
        }
    }
    
    public async Task<List<AnexoListModel>> GetDadosAnexos()
    {
        var dados = await connection.QueryAsync<AnexoListModel>(
            sql: @"
                select 
                  ANEXO, TIPO_DOC TIPO, CODIGO, DT_INICIO_VIGENCIA INIVIGENCIA, DT_FIM_VIGENCIA FIMVIGENCIA 
                from
                  RFC.TBL_CLASSIFICACAO_TRIBUTARIA_ANEXOS");

        return dados.ToList();
    }    
    
}