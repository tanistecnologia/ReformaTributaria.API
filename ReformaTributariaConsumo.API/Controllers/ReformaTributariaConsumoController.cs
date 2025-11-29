using Microsoft.AspNetCore.Mvc;

using ReformaTributaria.API.Model;
using ReformaTributaria.API.Services;
using ReformaTributaria.API.Services.Middlewares;
using ReformaTributaria.API.Utils.DTO;
using Swashbuckle.AspNetCore.Annotations;

using TClassificacaoTributaria;

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
    [SwaggerOperation("Atualiza tabela de Classificações Tributárias", "")]
    [Route("")]
    public async Task<IActionResult> PostData([FromBody] List<ClassificacaoTributariaModel> data)
    {
        logger.LogInformation("Dados recebidos: {DataCount}", data.Count);
        return Ok(await reformaTributariaService.InsereDados(data));
    }
    
    [HttpPost]
    [SwaggerOperation("Atualiza tabela de Classificações Tributárias", "")]
    [Route("RTC")]
    public async Task<IActionResult> PostData([FromBody] List<RtcClassificacaoTributariaModel> data)
    {
        logger.LogInformation("Dados recebidos: {DataCount}", data.Count);
        return Ok(await reformaTributariaService.InsereDadosRTC(data));
    }    

    [HttpGet]
    [SwaggerOperation("Lista tabela de Classificações Tributárias", "")]
    [Route("")]
    public async Task<ResponseDTO<List<ReformaTributariaListModel>>> GetData()
    {
        return new ResponseDTO<List<ReformaTributariaListModel>>()
        {
            Info = new BasicInfo<List<ReformaTributariaListModel>>()
            {
                Dados = await reformaTributariaService.GetDados()
            }
        };
    }

    [HttpPost]
    [SwaggerOperation("O campo tipo deve ser 1-NCM ou 2-NBS", "xxxxxxxxxxx")]
    [Route("Anexos")]
    public async Task<OkObjectResult> PostDataAnexos([FromBody] List<AnexoModel> data)
    {
        logger.LogInformation("Dados recebidos: {DataCount}", data.Count);
        return Ok(await reformaTributariaService.InsereDadosAnexos(data));
    }

    [HttpGet]
    [SwaggerOperation("O campo tipo deve ser 1-NCM ou 2-NBS", "xxxxxxxxxxx")]
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