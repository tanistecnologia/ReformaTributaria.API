using Dapper;
using Dapper.Transaction;

using ReformaTributaria.API.Model.Anexo;
using ReformaTributaria.API.Model.Lista;
using ReformaTributaria.API.Model.Post;
using ReformaTributaria.API.Utils;
using ReformaTributaria.API.Utils.Extensions;

using System.Data;

namespace ReformaTributaria.API.Services;

public class ReformaTributariaService(ILogger<ReformaTributariaService> logger, [FromKeyedServices("SQLServer")] IDbConnection connection)
{
    public async Task<string> InsereDadosRtc(List<RtcClassificacaoTributariaPostModel> listClassificacaoTributaria)
    {
        var rowsAffected = 0;
        var rowsCstAffected = 0;

        using var transaction = connection.BeginTransaction();
        try
        {
            var dataAtualizacaoTabela = DateTime.Now;
            await transaction.ExecuteAsync("delete from RTC.TBL_RTC_CLASSIFICACAO_TRIBUTARIA");
            await transaction.ExecuteAsync("delete from RTC.TBL_RTC_SITUACAO_TRIBUTARIA");

            foreach (var ctrib in listClassificacaoTributaria)
            {
                var parameters = new DynamicParameters();
                parameters.Add("RST_COD_CST", ctrib.Cst.CodigoSituacaoTributaria, dbType: DbType.String);
                var qtdeCst = await transaction.QuerySingleOrDefaultAsync<int>(
                    sql: "select count(*) from RTC.TBL_RTC_SITUACAO_TRIBUTARIA C where C.RST_COD_CST = @RST_COD_CST",
                    param: parameters
                );

                if (qtdeCst == 0)
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
                    parameters.Add("RST_DT_ATUALIZACAO_TABELA", dataAtualizacaoTabela, dbType: DbType.Date);

                    rowsCstAffected += await transaction.ExecuteAsync(
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
                parameters.Add("RTA_COD_ANEXO", ctrib.Anexo, dbType: DbType.String);
                parameters.Add("RCT_DS_CLASS_TRIB", ctrib.DescricaoClassificacaoTributaria, dbType: DbType.String);
                parameters.Add("RCT_NOME_CLASS_TRIB", ctrib.NomeClassTrib, dbType: DbType.String);
                parameters.Add("RCT_LC_REDACAO", ctrib.LcRedacao, dbType: DbType.String);
                parameters.Add("RCT_LC_214_25", ctrib.Lc214_25, dbType: DbType.String);
                parameters.Add("RCT_TIPO_ALIQUOTA", ctrib.TipoAliquota, dbType: DbType.String);

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

                parameters.Add("RCT_LINK_LEGISLACAO", ctrib.Link, dbType: DbType.String);

                parameters.Add("RCT_DT_INI_VIGENCIA", ctrib.DIniVig.ToDateOnly(), dbType: DbType.Date);
                parameters.Add("RCT_DT_FIM_VIGENCIA", ctrib.DFimVig.ToDateOnly(), dbType: DbType.Date);
                parameters.Add("RCT_DT_ATUALIZACAO", ctrib.DataAtualizacao.ToDateOnly(), dbType: DbType.Date);

                rowsAffected += await transaction.ExecuteAsync(
                    sql: @"
							INSERT INTO RTC.TBL_RTC_CLASSIFICACAO_TRIBUTARIA
								(RCT_COD_CLASS_TRIB, RST_COD_CST, RTA_COD_ANEXO, RCT_DS_CLASS_TRIB, RCT_NOME_CLASS_TRIB, RCT_LC_REDACAO, RCT_LC_214_25,
                                RCT_TIPO_ALIQUOTA, RCT_PERC_RED_IBS, RCT_PERC_RED_CBS, RCT_IND_GTRIBREGULAR, RCT_IND_GCREDPRESOPER,
                                RCT_IND_GMONOPADRAO, RCT_IND_GMONORETEN, RCT_IND_GMONORET, RCT_IND_GMONODIF, RCT_IND_GESTORNOCRED, 
                                RCT_IND_NFEABI, RCT_IND_NFE, RCT_IND_NFCE, RCT_IND_CTE, RCT_IND_CTEOS, RCT_IND_BPE, RCT_IND_BPETA, 
                                RCT_IND_BPETM, RCT_IND_NF3E, RCT_IND_NFSE, RCT_IND_NFSEVIA, RCT_IND_NFCOM, RCT_IND_NFAG, RCT_IND_NFGAS,
                                RCT_IND_DERE, RCT_LINK_LEGISLACAO, RCT_DT_INI_VIGENCIA, RCT_DT_FIM_VIGENCIA, RCT_DT_ATUALIZACAO)
							VALUES
								(@RCT_COD_CLASS_TRIB, @RST_COD_CST, @RTA_COD_ANEXO, @RCT_DS_CLASS_TRIB, @RCT_NOME_CLASS_TRIB, @RCT_LC_REDACAO, @RCT_LC_214_25,
                                @RCT_TIPO_ALIQUOTA, @RCT_PERC_RED_IBS, @RCT_PERC_RED_CBS, @RCT_IND_GTRIBREGULAR, @RCT_IND_GCREDPRESOPER,
                                @RCT_IND_GMONOPADRAO, @RCT_IND_GMONORETEN, @RCT_IND_GMONORET, @RCT_IND_GMONODIF, @RCT_IND_GESTORNOCRED, 
                                @RCT_IND_NFEABI, @RCT_IND_NFE, @RCT_IND_NFCE, @RCT_IND_CTE, @RCT_IND_CTEOS, @RCT_IND_BPE, @RCT_IND_BPETA, 
                                @RCT_IND_BPETM, @RCT_IND_NF3E, @RCT_IND_NFSE, @RCT_IND_NFSEVIA, @RCT_IND_NFCOM, @RCT_IND_NFAG, @RCT_IND_NFGAS,
                                @RCT_IND_DERE, @RCT_LINK_LEGISLACAO, @RCT_DT_INI_VIGENCIA, @RCT_DT_FIM_VIGENCIA, @RCT_DT_ATUALIZACAO)",
                    param: parameters
                );

                logger.LogInformation(
                    "Registros inseridos: {CodigoClassificacaoTributaria} CST: {RowsCSTAffected} CCLASTRIB: {RowsAffected}",
                    ctrib.CodigoClassificacaoTributaria.Trim(), rowsCstAffected, rowsAffected);
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

    public async Task<List<RtcClassificacaoTributariaListaModel>> GetDados()
    {
        var dados = await connection.QueryAsync<RtcClassificacaoTributariaListaModel, RtcSituacaoTributariaListaModel, RtcClassificacaoTributariaListaModel>(
            sql: @"
                select
                  RCT.RCT_COD_CLASS_TRIB, RCT.RTA_COD_ANEXO, RCT.RCT_DS_CLASS_TRIB, RCT.RCT_NOME_CLASS_TRIB,
                  RCT.RCT_LC_REDACAO, RCT.RCT_LC_214_25, RCT.RCT_TIPO_ALIQUOTA, RCT.RCT_PERC_RED_IBS,
                  RCT.RCT_PERC_RED_CBS, RCT.RCT_IND_GTRIBREGULAR, RCT.RCT_IND_GCREDPRESOPER,
                  RCT.RCT_IND_GMONOPADRAO, RCT.RCT_IND_GMONORETEN, RCT.RCT_IND_GMONORET, RCT.RCT_IND_GMONODIF,
                  RCT.RCT_IND_GESTORNOCRED, RCT.RCT_IND_NFEABI, RCT.RCT_IND_NFE, RCT.RCT_IND_NFCE, RCT.RCT_IND_CTE,
                  RCT.RCT_IND_CTEOS, RCT.RCT_IND_BPE, RCT.RCT_IND_BPETA, RCT.RCT_IND_BPETM, RCT.RCT_IND_NF3E,
                  RCT.RCT_IND_NFSE, RCT.RCT_IND_NFSEVIA, RCT.RCT_IND_NFCOM, RCT.RCT_IND_NFAG, RCT.RCT_IND_NFGAS,
                  RCT.RCT_IND_DERE, RCT.RCT_LINK_LEGISLACAO, cast(RCT.RCT_DT_INI_VIGENCIA as DATE) RCT_DT_INI_VIGENCIA, 
                  cast(RCT.RCT_DT_FIM_VIGENCIA as DATE) RCT_DT_FIM_VIGENCIA, RCT.RCT_DT_ATUALIZACAO,

                  RST.RST_COD_CST, RST.RST_DS_SITUACAO_TRIBUTARIA, RST.RST_IND_GIBSCBS, RST.RST_IND_GIBSCBSMONO,
                  RST.RST_IND_GRED, RST.RST_IND_GDIF, RST.RST_IND_GTRANSFCRED, RST.RST_IND_GCREDPRESIBSZFM,
                  RST.RST_IND_REDUTORBC, RST.RST_DT_ATUALIZACAO_TABELA
                from
                  RTC.TBL_RTC_CLASSIFICACAO_TRIBUTARIA RCT
                  inner join RTC.TBL_RTC_SITUACAO_TRIBUTARIA RST on RST.RST_COD_CST = RCT.RST_COD_CST",
            map: (cclastrib, cst) =>
            {
                cclastrib.CST = cst;
                return cclastrib;
            },
            splitOn: "RST_COD_CST");

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
                await transaction.ExecuteAsync("delete from RTC.TBL_RTC_ANEXOS");

                foreach (var anexo in anexos)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("RTA_COD_ANEXO", anexo.Anexo, dbType: DbType.Int64);
                    parameters.Add("RTA_TIPO_DOC", anexo.Tipo, dbType: DbType.Int16);
                    parameters.Add("RTA_CODIGO", anexo.Codigo, dbType: DbType.String);
                    parameters.Add("RTA_DT_INICIO_VIGENCIA", anexo.IniVigencia.ToDateOnly(), dbType: DbType.Date);
                    parameters.Add("RTA_DT_FIM_VIGENCIA", anexo.FimVigencia.ToDateOnly(), dbType: DbType.Date);
                    parameters.Add("RTA_DT_ATUALIZACAO_TABELA", DataAtualizacaoTabela, dbType: DbType.Date);

                    rowsAffected += await transaction.ExecuteAsync(
                        sql: @"
							INSERT INTO RTC.TBL_RTC_ANEXOS
							  (RTA_COD_ANEXO, RTA_TIPO_DOC, RTA_CODIGO, RTA_DT_INICIO_VIGENCIA, RTA_DT_FIM_VIGENCIA, RTA_DT_ATUALIZACAO_TABELA)
							VALUES
							  (@RTA_COD_ANEXO, @RTA_TIPO_DOC, @RTA_CODIGO, @RTA_DT_INICIO_VIGENCIA, @RTA_DT_FIM_VIGENCIA, @RTA_DT_ATUALIZACAO_TABELA)",
                        param: parameters
                    );

                    logger.LogInformation(
                        "Registro inserido: {Anexo} - {Tipo} - {Codigo}/{RowsAffected}",
                        anexo.Anexo, anexo.Tipo, anexo.Codigo, rowsAffected);
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
                  RTA_COD_ANEXO ANEXO, RTA_TIPO_DOC TIPO, RTA_CODIGO CODIGO, RTA_DT_INICIO_VIGENCIA INIVIGENCIA, 
                  RTA_DT_FIM_VIGENCIA FIMVIGENCIA, RTA_DT_ATUALIZACAO_TABELA ATUALIZADO_EM 
                from
                  RTC.TBL_RTC_ANEXOS");

        return [.. dados];
    }

}