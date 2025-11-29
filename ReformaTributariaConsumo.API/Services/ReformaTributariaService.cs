using Dapper;
using Dapper.Transaction;

using ReformaTributaria.API.Model;
using ReformaTributaria.API.Utils;

using System.Data;

using TClassificacaoTributaria;

namespace ReformaTributaria.API.Services;

public class ReformaTributariaService(ILogger<ReformaTributariaService> logger, [FromKeyedServices("SQLServer")] IDbConnection connection)
{
    public async Task<string> InsereDados(List<ClassificacaoTributariaModel> listClassificacaoTributaria)
    {
        var rowsAffected = 0;
        var rowsCSTAffected = 0;

        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                var DataAtualizacaoTabela = DateTime.Now;
                await transaction.ExecuteAsync("delete from RTC.TBL_CLASSIFICACAO_TRIBUTARIA");
                await transaction.ExecuteAsync("delete from RTC.TBL_SITUACAO_TRIBUTARIA");

                foreach (var ctrib in listClassificacaoTributaria)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("COD_CST", ctrib.CódigoDaSituaçãoTributária, dbType: DbType.String);
                    var qtdeCST = await transaction.QuerySingleOrDefaultAsync<int>(
                        sql: "select count(*) from RTC.TBL_SITUACAO_TRIBUTARIA C where C.COD_CST = @COD_CST",
                        param: parameters
                    );

                    if (qtdeCST == 0)
                    {
                        parameters = new DynamicParameters();
                        parameters.Add("COD_CST", ctrib.CódigoDaSituaçãoTributária, dbType: DbType.String);
                        parameters.Add("DS_SITUACAO_TRIBUTARIA", ctrib.DescriçãoDoCódigoDaSituaçãoTributária, dbType: DbType.String);
                        parameters.Add("EXIGE_TRIB", ctrib.ExigeTributação.SafeSubstring(0, 1), dbType: DbType.String);
                        parameters.Add("RED_BC_CST", ctrib.ReduçãoBcCst.SafeSubstring(0, 1), dbType: DbType.String);
                        parameters.Add("RED_ALIQUOTA", ctrib.ReduçãoAlíquota.SafeSubstring(0, 1), dbType: DbType.String);
                        parameters.Add("TRANSF_CREDITO", ctrib.TransferênciaCrédito.SafeSubstring(0, 1), dbType: DbType.String);
                        parameters.Add("DIFERIMENTO", ctrib.Diferimento.SafeSubstring(0, 1), dbType: DbType.String);
                        parameters.Add("MONOFASICA", ctrib.Monofásica.SafeSubstring(0, 1), dbType: DbType.String);
                        parameters.Add("DT_ATUALIZACAO_TABELA", DataAtualizacaoTabela, dbType: DbType.Date);

                        rowsCSTAffected += await transaction.ExecuteAsync(
                            sql: @"
                                insert into RTC.TBL_SITUACAO_TRIBUTARIA(COD_CST, DS_SITUACAO_TRIBUTARIA, EXIGE_TRIB, RED_BC_CST,
                                                                        RED_ALIQUOTA, TRANSF_CREDITO, DIFERIMENTO, MONOFASICA,
                                                                        DT_ATUALIZACAO_TABELA)
                                values (@COD_CST, @DS_SITUACAO_TRIBUTARIA, @EXIGE_TRIB, @RED_BC_CST, @RED_ALIQUOTA,
                                        @TRANSF_CREDITO, @DIFERIMENTO, @MONOFASICA, @DT_ATUALIZACAO_TABELA)",
                            param: parameters
                        );
                    }


                    parameters = new DynamicParameters();
                    parameters.Add("COD_CLASS_TRIB", ctrib.CódigoDaClassificaçãoTributária.Trim(), dbType: DbType.String);
                    parameters.Add("COD_CST", ctrib.CódigoDaSituaçãoTributária.Trim(), dbType: DbType.String);
                    parameters.Add("DS_CLASS_TRIB", ctrib.DescriçãoDoCódigoDaClassificaçãoTributária, dbType: DbType.String);
                    parameters.Add("NOME_CLASS_TRIB", ctrib.NomeDaClassificaçãoTributária, dbType: DbType.String);
                    parameters.Add("REDACAO_LC_214_2025", ctrib.RedaçãoLc2142025, dbType: DbType.String);
                    parameters.Add("ARTIGO_LC_214_2025", ctrib.ArtigoLc2142025, dbType: DbType.String);
                    parameters.Add("TIPO_ALIQUOTA", ctrib.TipoDeAlíquota, dbType: DbType.String);
                    parameters.Add("PERC_RED_IBS", ctrib.PercentualReduçãoIbs, dbType: DbType.Decimal);
                    parameters.Add("PERC_RED_CBS", ctrib.PercentualReduçãoCbs, dbType: DbType.Decimal);
                    parameters.Add("RED_BC", ctrib.ReduçãoBc.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("TRIB_REGULAR", ctrib.TributaçãoRegular.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("CRED_PRESUMIDO", ctrib.CréditoPresumido.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("CRED_PARA", ctrib.CréditoPara, dbType: DbType.String);
                    parameters.Add("IND_RED_BC", ctrib.IndicadorRedutorBc.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("IND_MONO_PADRAO", ctrib.IndicadorMonofásicaPadrão.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("IND_MONO_RETENCAO", ctrib.IndicadorMonofásicaRetenção.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("IND_MONO_RETIDO", ctrib.IndicadorMonofásicaRetido.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("IND_MONO_DIFERIMENTO", ctrib.IndicadorMonofásicaDiferimento.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("IND_ESTORNO_CREDITO", ctrib.IndicadorEstornoCrédito.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("IND_CRED_PRESUMIDO_ZFM", ctrib.IndicadorCréditoPresumidoZfm.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("IND_AJUSTE_COMPETENCIA", ctrib.IndicadorAjusteCompetência.SafeSubstring(0, 1), dbType: DbType.String);

                    if (ctrib.DataInícioVigência != string.Empty)
                    {
                        var date = DateOnly.ParseExact(ctrib.DataInícioVigência, "yyyy-MM-dd");
                        parameters.Add("DT_INI_VIGENCIA", date, dbType: DbType.Date);
                    }
                    else
                    {
                        parameters.Add("DT_INI_VIGENCIA", DBNull.Value, dbType: DbType.Date);
                    }

                    if (ctrib.DataFimVigência != string.Empty)
                    {
                        var date = DateOnly.ParseExact(ctrib.DataFimVigência, "yyyy-MM-dd");
                        parameters.Add("DT_FIM_VIGENCIA", date, dbType: DbType.Date);
                    }
                    else
                    {
                        parameters.Add("DT_FIM_VIGENCIA", DBNull.Value, dbType: DbType.Date);
                    }

                    if (ctrib.DataAtualização != string.Empty)
                    {
                        var date = DateOnly.ParseExact(ctrib.DataAtualização, "yyyy-MM-dd");
                        parameters.Add("DT_ATUALIZACAO", date, dbType: DbType.Date);
                    }
                    else
                    {
                        parameters.Add("DT_ATUALIZACAO", DBNull.Value, dbType: DbType.Date);
                    }

                    parameters.Add("ANEXO", ctrib.Anexo, dbType: DbType.String);
                    parameters.Add("LINK_LEGISLACAO", ctrib.LinkLegislação, dbType: DbType.String);
                    parameters.Add("APLICA_NFE_ABI", ctrib.AplicaNFeAbi.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFE", ctrib.AplicaNFe.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFCE", ctrib.AplicaNfCe.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_CTE", ctrib.AplicaCTe.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_CTE_OS", ctrib.AplicaCTeOs.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_BPE", ctrib.AplicaBPe.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_BPE_TA", ctrib.AplicaBPeTransporteAéreo.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_BPE_TM", ctrib.AplicaBPeTransporteMarítimo.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NF3E", ctrib.AplicaNf3E.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFSE", ctrib.AplicaNfSe.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFSE_VIA", ctrib.AplicaNfSeVia.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFCOM", ctrib.AplicaNfCom.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFAG", ctrib.AplicaNfAg.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_NFGAS", ctrib.AplicaNfGas.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("APLICA_DERE", ctrib.AplicaDere.SafeSubstring(0, 1), dbType: DbType.String);
                    parameters.Add("DT_ATUALIZACAO_TABELA", DataAtualizacaoTabela, dbType: DbType.Date);

                    rowsAffected += await transaction.ExecuteAsync(
                        sql: @"
							INSERT INTO RTC.TBL_CLASSIFICACAO_TRIBUTARIA
								(COD_CLASS_TRIB, COD_CST, DS_CLASS_TRIB, NOME_CLASS_TRIB, REDACAO_LC_214_2025, ARTIGO_LC_214_2025, TIPO_ALIQUOTA, 
                                 PERC_RED_IBS, PERC_RED_CBS, RED_BC, TRIB_REGULAR, CRED_PRESUMIDO, CRED_PARA, IND_RED_BC, IND_MONO_PADRAO, IND_MONO_RETENCAO, 
                                 IND_MONO_RETIDO, IND_MONO_DIFERIMENTO, IND_ESTORNO_CREDITO, IND_CRED_PRESUMIDO_ZFM, IND_AJUSTE_COMPETENCIA, DT_INI_VIGENCIA, 
                                 DT_FIM_VIGENCIA, DT_ATUALIZACAO, ANEXO, LINK_LEGISLACAO, APLICA_NFE_ABI, APLICA_NFE, APLICA_NFCE, APLICA_CTE, APLICA_CTE_OS, 
                                 APLICA_BPE, APLICA_BPE_TA, APLICA_BPE_TM, APLICA_NF3E, APLICA_NFSE, APLICA_NFSE_VIA, APLICA_NFCOM, APLICA_NFAG, APLICA_NFGAS, 
                                 APLICA_DERE, DT_ATUALIZACAO_TABELA)
							VALUES
								(@COD_CLASS_TRIB, @COD_CST, @DS_CLASS_TRIB, @NOME_CLASS_TRIB, @REDACAO_LC_214_2025, @ARTIGO_LC_214_2025, @TIPO_ALIQUOTA, 
                                 @PERC_RED_IBS, @PERC_RED_CBS, @RED_BC, @TRIB_REGULAR, @CRED_PRESUMIDO, @CRED_PARA, @IND_RED_BC, @IND_MONO_PADRAO, @IND_MONO_RETENCAO, 
                                 @IND_MONO_RETIDO, @IND_MONO_DIFERIMENTO, @IND_ESTORNO_CREDITO, @IND_CRED_PRESUMIDO_ZFM, @IND_AJUSTE_COMPETENCIA, @DT_INI_VIGENCIA, 
                                 @DT_FIM_VIGENCIA, @DT_ATUALIZACAO, @ANEXO, @LINK_LEGISLACAO, @APLICA_NFE_ABI, @APLICA_NFE, @APLICA_NFCE, @APLICA_CTE, @APLICA_CTE_OS, 
                                 @APLICA_BPE, @APLICA_BPE_TA, @APLICA_BPE_TM, @APLICA_NF3E, @APLICA_NFSE, @APLICA_NFSE_VIA, @APLICA_NFCOM, @APLICA_NFAG, @APLICA_NFGAS, 
                                 @APLICA_DERE, @DT_ATUALIZACAO_TABELA)",
                        param: parameters
                    );

                    var logTxt =
                        $"Registros inseridos: {ctrib.CódigoDaClassificaçãoTributária.Trim()} CST: {rowsCSTAffected} CCLASTRIB: {rowsAffected}";
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


    public async Task<string> InsereDadosRTC(List<RtcClassificacaoTributariaModel> listClassificacaoTributaria)
    {
        var rowsAffected = 0;
        var rowsCSTAffected = 0;

        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                var DataAtualizacaoTabela = DateTime.Now;
                await transaction.ExecuteAsync("delete from RTC.TBL_RTC_CLASSIFICACAO_TRIBUTARIA");
                await transaction.ExecuteAsync("delete from RTC.TBL_RTC_SITUACAO_TRIBUTARIA");

                foreach (var ctrib in listClassificacaoTributaria)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("RST_COD_CST", ctrib.Cst.CodigoSituacaoTributaria, dbType: DbType.String);
                    var qtdeCST = await transaction.QuerySingleOrDefaultAsync<int>(
                        sql: "select count(*) from RTC.TBL_RTC_SITUACAO_TRIBUTARIA C where C.RST_COD_CST = @RST_COD_CST",
                        param: parameters
                    );

                    if (qtdeCST == 0)
                    {
                        parameters = new DynamicParameters();
                        parameters.Add("RST_COD_CST", ctrib.Cst.CodigoSituacaoTributaria, dbType: DbType.String);
                        parameters.Add("RST_DS_SITUACAO_TRIBUTARIA", ctrib.Cst.DescricaoSituacaoTributaria, dbType: DbType.String);
                        parameters.Add("RST_IND_GIBSCBS", ctrib.Cst.IndGIbsCbs.ToSN(), dbType: DbType.String);
                        parameters.Add("RST_IND_GIBSCBSMONO", ctrib.Cst.IndGIbsCbsMono.ToSN(), dbType: DbType.String);
                        parameters.Add("RST_IND_GRED", ctrib.Cst.IndGRed.ToSN(), dbType: DbType.String);
                        parameters.Add("RST_IND_GDIF", ctrib.Cst.IndGDif.ToSN(), dbType: DbType.String);
                        parameters.Add("RST_IND_GTRANSFCRED", ctrib.Cst.IndGTransfCred.ToSN(), dbType: DbType.String);
                        parameters.Add("RST_IND_GCREDPRESIBSZFM", ctrib.Cst.IndGCredPresIbsZfm.ToSN(), dbType: DbType.String);
                        parameters.Add("RST_IND_REDUTORBC", ctrib.Cst.IndRedutorBc.ToSN(), dbType: DbType.String);
                        parameters.Add("RST_DT_ATUALIZACAO_TABELA", DataAtualizacaoTabela, dbType: DbType.Date);

                        rowsCSTAffected += await transaction.ExecuteAsync(
                            sql: @"
                                insert into RTC.TBL_RTC_SITUACAO_TRIBUTARIA(
                                    RST_COD_CST, RST_DS_SITUACAO_TRIBUTARIA, RST_IND_GIBSCBS, RST_IND_GIBSCBSMONO, RST_IND_GRED, RST_IND_GDIF,
                                    RST_IND_GTRANSFCRED, RST_IND_GCREDPRESIBSZFM, RST_IND_REDUTORBC, RST_DT_ATUALIZACAO_TABELA)
                                values (@RST_COD_CST, @RST_DS_SITUACAO_TRIBUTARIA, @RST_IND_GIBSCBS, @RST_IND_GIBSCBSMONO, @RST_IND_GRED, @RST_IND_GDIF,
                                        @RST_IND_GTRANSFCRED, @RST_IND_GCREDPRESIBSZFM, @RST_IND_REDUTORBC, @RST_DT_ATUALIZACAO_TABELA)",
                            param: parameters
                        );
                    }

                    parameters = new DynamicParameters();
                    parameters.Add("RCT_COD_CLASS_TRIB", ctrib.CodigoClassificacaoTributaria.Trim(), dbType: DbType.String);
                    parameters.Add("RST_COD_CST", ctrib.Cst.CodigoSituacaoTributaria.Trim(), dbType: DbType.String);
                    parameters.Add("RCT_DS_CLASS_TRIB", ctrib.DescricaoClassificacaoTributaria, dbType: DbType.String);
                    parameters.Add("RCT_NOME_CLASS_TRIB", ctrib.NomeClassTrib, dbType: DbType.String);
                    parameters.Add("RCT_LC_REDACAO", ctrib.LcRedacao, dbType: DbType.String);

                    parameters.Add("RCT_PERC_RED_IBS", ctrib.PRedIbs, dbType: DbType.Decimal);
                    parameters.Add("RCT_PERC_RED_CBS", ctrib.PRedCbs, dbType: DbType.Decimal);

                    parameters.Add("RCT_IND_GTRIBREGULAR", ctrib.IndGTribRegular.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_GCREDPRESOPER", ctrib.IndGCredPresOper.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_GMONOPADRAO", ctrib.IndGMonoPadrao.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_GMONORETEN", ctrib.IndGMonoReten.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_GMONORET", ctrib.IndGMonoRet.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_GMONODIF", ctrib.IndGMonoDif.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_GESTORNOCRED", ctrib.IndGEstornoCred.ToSN(), dbType: DbType.String);

                    parameters.Add("RCT_IND_NFEABI", ctrib.IndNFeAbi.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_NFE", ctrib.IndNFe.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_NFCE", ctrib.IndNfCe.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_CTE", ctrib.IndCTe.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_CTEOS", ctrib.IndCTeOs.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_BPE", ctrib.IndBPe.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_BPETA", ctrib.IndBPeTa.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_BPETM", ctrib.IndBPeTm.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_NF3E", ctrib.IndNf3E.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_NFSE", ctrib.IndNfSe.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_NFSEVIA", ctrib.IndNfSeVia.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_NFCOM", ctrib.IndNfCom.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_NFAG", ctrib.IndNfAg.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_NFGAS", ctrib.IndNfGas.ToSN(), dbType: DbType.String);
                    parameters.Add("RCT_IND_DERE", ctrib.IndDere.ToSN(), dbType: DbType.String);

                    //if (ctrib.DataInícioVigência != string.Empty)
                    //{
                    //    var date = DateOnly.ParseExact(ctrib.DataInícioVigência, "yyyy-MM-dd");
                    //    parameters.Add("DT_INI_VIGENCIA", date, dbType: DbType.Date);
                    //}
                    //else
                    //{
                    //    parameters.Add("DT_INI_VIGENCIA", DBNull.Value, dbType: DbType.Date);
                    //}

                    //if (ctrib.DataFimVigência != string.Empty)
                    //{
                    //    var date = DateOnly.ParseExact(ctrib.DataFimVigência, "yyyy-MM-dd");
                    //    parameters.Add("DT_FIM_VIGENCIA", date, dbType: DbType.Date);
                    //}
                    //else
                    //{
                    //    parameters.Add("DT_FIM_VIGENCIA", DBNull.Value, dbType: DbType.Date);
                    //}

                    //if (ctrib.DataAtualizacao != string.Empty)
                    //{
                    //    var date = DateOnly.ParseExact(ctrib.DataAtualizacao, "yyyy-MM-dd");
                    //    parameters.Add("DT_ATUALIZACAO", date, dbType: DbType.Date);
                    //}
                    //else
                    //{
                    //    parameters.Add("DT_ATUALIZACAO", DBNull.Value, dbType: DbType.Date);
                    //}

                    parameters.Add("RCT_DT_INI_VIGENCIA", ctrib.DIniVig, dbType: DbType.Date);
                    parameters.Add("RCT_DT_FIM_VIGENCIA", ctrib.DFimVig, dbType: DbType.Date);
                    parameters.Add("RCT_DT_ATUALIZACAO", ctrib.DataAtualizacao, dbType: DbType.Date);

                    rowsAffected += await transaction.ExecuteAsync(
                        sql: @"
							INSERT INTO RTC.TBL_CLASSIFICACAO_TRIBUTARIA
								(RCT_COD_CLASS_TRIB, RST_COD_CST, RCT_DS_CLASS_TRIB, RCT_NOME_CLASS_TRIB, RCT_LC_REDACAO, RCT_LC_214_25,
                                RCT_TIPO_ALIQUOTA, RCT_PERC_RED_IBS, RCT_PERC_RED_CBS, RCT_IND_GTRIBREGULAR, RCT_IND_GCREDPRESOPER,
                                RCT_IND_GMONOPADRAO, RCT_IND_GMONORETEN, RCT_IND_GMONORET, RCT_IND_GMONODIF, RCT_IND_GESTORNOCRED, 
                                RCT_IND_NFEABI, RCT_IND_NFE, RCT_IND_NFCE, RCT_IND_CTE, RCT_IND_CTEOS, RCT_IND_BPE, RCT_IND_BPETA, 
                                RCT_IND_BPETM, RCT_IND_NF3E, RCT_IND_NFSE, RCT_IND_NFSEVIA, RCT_IND_NFCOM, RCT_IND_NFAG, RCT_IND_NFGAS,
                                RCT_IND_DERE, RCT_DT_INI_VIGENCIA, RCT_DT_FIM_VIGENCIA, RCT_DT_ATUALIZACAO, RTA_ANEXO, RCT_LINK_LEGISLACAO)
							VALUES
								(@RCT_COD_CLASS_TRIB, @RST_COD_CST, @RCT_DS_CLASS_TRIB, @RCT_NOME_CLASS_TRIB, @RCT_LC_REDACAO, @RCT_LC_214_25,
                                @RCT_TIPO_ALIQUOTA, @RCT_PERC_RED_IBS, @RCT_PERC_RED_CBS, @RCT_IND_GTRIBREGULAR, @RCT_IND_GCREDPRESOPER,
                                @RCT_IND_GMONOPADRAO, @RCT_IND_GMONORETEN, @RCT_IND_GMONORET, @RCT_IND_GMONODIF, @RCT_IND_GESTORNOCRED, 
                                @RCT_IND_NFEABI, @RCT_IND_NFE, @RCT_IND_NFCE, @RCT_IND_CTE, @RCT_IND_CTEOS, @RCT_IND_BPE, @RCT_IND_BPETA, 
                                @RCT_IND_BPETM, @RCT_IND_NF3E, @RCT_IND_NFSE, @RCT_IND_NFSEVIA, @RCT_IND_NFCOM, @RCT_IND_NFAG, @RCT_IND_NFGAS,
                                @RCT_IND_DERE, @RTA_ANEXO, @RCT_LINK_LEGISLACAO, @RCT_DT_INI_VIGENCIA, @RCT_DT_FIM_VIGENCIA, @RCT_DT_ATUALIZACAO)",
                        param: parameters
                    );

                    var logTxt =
                        $"Registros inseridos: {ctrib.CódigoDaClassificaçãoTributária.Trim()} CST: {rowsCSTAffected} CCLASTRIB: {rowsAffected}";
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
                  CST.COD_CST, CST.DS_SITUACAO_TRIBUTARIA DS_SIT_TRIBUTARIA, CST.EXIGE_TRIB, CST.RED_BC_CST, CST.RED_ALIQUOTA,
                  CST.TRANSF_CREDITO, CST.DIFERIMENTO, CST.MONOFASICA, CT.COD_CLASS_TRIB, CT.DS_CLASS_TRIB,
                  CT.NOME_CLASS_TRIB, CT.REDACAO_LC_214_2025, CT.ARTIGO_LC_214_2025, CT.TIPO_ALIQUOTA,
                  CT.PERC_RED_IBS, CT.PERC_RED_CBS, CT.RED_BC, CT.TRIB_REGULAR, CT.CRED_PRESUMIDO, CT.CRED_PARA,
                  CT.IND_RED_BC, CT.IND_MONO_PADRAO, CT.IND_MONO_RETENCAO, CT.IND_MONO_RETIDO,
                  CT.IND_MONO_DIFERIMENTO, CT.IND_ESTORNO_CREDITO, CT.IND_CRED_PRESUMIDO_ZFM,
                  CT.IND_AJUSTE_COMPETENCIA, CT.DT_INI_VIGENCIA, CT.DT_FIM_VIGENCIA, CT.DT_ATUALIZACAO, CT.ANEXO,
                  CT.LINK_LEGISLACAO, CT.APLICA_NFE_ABI, CT.APLICA_NFE, CT.APLICA_NFCE, CT.APLICA_CTE,
                  CT.APLICA_CTE_OS, CT.APLICA_BPE, CT.APLICA_BPE_TA, CT.APLICA_BPE_TM, CT.APLICA_NF3E,
                  CT.APLICA_NFSE, CT.APLICA_NFSE_VIA, CT.APLICA_NFCOM, CT.APLICA_NFAG, CT.APLICA_NFGAS,
                  CT.APLICA_DERE, CT.DT_ATUALIZACAO_TABELA
                from
                  RTC.TBL_CLASSIFICACAO_TRIBUTARIA CT
                  inner join RTC.TBL_SITUACAO_TRIBUTARIA CST on CST.COD_CST = CT.COD_CST");

        return [.. dados];
    }

    public async Task<string> InsereDadosAnexos(List<AnexoModel> anexos)
    {
        var rowsAffected = 0;

        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                var DataAtualizacaoTabela = DateTime.Now;
                await transaction.ExecuteAsync("delete from RTC.TBL_CLASSIFICACAO_TRIBUTARIA_ANEXOS");

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

                    parameters.Add("DT_ATUALIZACAO_TABELA", DataAtualizacaoTabela, dbType: DbType.Date);

                    rowsAffected += await transaction.ExecuteAsync(
                        sql: @"
							INSERT INTO RTC.TBL_CLASSIFICACAO_TRIBUTARIA_ANEXOS
							  (ANEXO, TIPO_DOC, CODIGO, DT_INICIO_VIGENCIA, DT_FIM_VIGENCIA, DT_ATUALIZACAO_TABELA)
							VALUES
							  (@ANEXO, @TIPO_DOC, @CODIGO, @DT_INICIO_VIGENCIA, @DT_FIM_VIGENCIA, @DT_ATUALIZACAO_TABELA)",
                        param: parameters
                    );

                    var logTxt =
                        $"Registro inserido: {anexo.Anexo} - {anexo.Tipo} - {anexo.Codigo}/{rowsAffected}";
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
                  ANEXO, TIPO_DOC TIPO, CODIGO, DT_INICIO_VIGENCIA INIVIGENCIA, DT_FIM_VIGENCIA FIMVIGENCIA, DT_ATUALIZACAO_TABELA 
                from
                  RTC.TBL_CLASSIFICACAO_TRIBUTARIA_ANEXOS");

        return [.. dados];
    }

}