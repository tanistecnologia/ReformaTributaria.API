using Microsoft.AspNetCore.Mvc;
using ReformaTributaria.API.Model.Anexo;
using ReformaTributaria.API.Model.Lista;
using ReformaTributaria.API.Model.Post;
using ReformaTributaria.API.Services;
using ReformaTributaria.API.Services.Middlewares;
using ReformaTributaria.API.Utils.DTO;

using Swashbuckle.AspNetCore.Annotations;

namespace ReformaTributaria.API.Controllers;

[ApiController]
[ApiKeyAuth]
[SwaggerTag("Utilizada para gerenciar a Classificacao Tributaria.")]
[Produces("application/json")]
[Route("api/hubtributario/v10/[controller]/ClassificacaoTributaria")]
public class ReformaTributariaConsumoController(
    ILogger<ReformaTributariaConsumoController> logger,
    ReformaTributariaService reformaTributariaService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation("Atualiza as tabelas de Classificações Tributárias", "")]
    [Route("")]
    public async Task<IActionResult> PostData([FromBody] List<RtcClassificacaoTributariaPostModel> data)
    {
        logger.LogInformation("Dados recebidos: {DataCount}", data.Count);
        return Ok(await reformaTributariaService.InsereDadosRtc(data));
    }

    [HttpGet]
    [SwaggerOperation("Lista tabela de Classificações Tributárias", "")]
    [Route("")]
    public async Task<ResponseDTO<List<RtcClassificacaoTributariaListaModel>>> GetData()
    {
        return new ResponseDTO<List<RtcClassificacaoTributariaListaModel>>
        {
            Info = new BasicInfo<List<RtcClassificacaoTributariaListaModel>>
            {
                Dados = await reformaTributariaService.GetDados()
            }
        };
    }

    [HttpPost]
    [SwaggerOperation("Lista de Anexos das Classificações Tributárias", "O campo tipo deve ser 1-NCM ou 2-NBS")]
    [Route("Anexos")]
    public async Task<OkObjectResult> PostDataAnexos([FromBody] List<AnexoModel> data)
    {
        logger.LogInformation("Dados recebidos: {DataCount}", data.Count);
        return Ok(await reformaTributariaService.InsereDadosAnexos(data));
    }

    [HttpGet]
    [SwaggerOperation("Lista de Anexos das Classificações Tributárias", "")]
    [Route("Anexos")]
    public async Task<ResponseDTO<List<AnexoListModel>>> GetDataAnexos()
    {
        return new ResponseDTO<List<AnexoListModel>>()
        {
            Info = new BasicInfo<List<AnexoListModel>>()
            {
                Dados = await reformaTributariaService.GetDadosAnexos()
            }
        };
    }
}