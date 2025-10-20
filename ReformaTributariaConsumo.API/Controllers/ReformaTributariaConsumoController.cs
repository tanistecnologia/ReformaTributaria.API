using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ReformaTributaria.API.Model;
using ReformaTributaria.API.Services;

using Swashbuckle.AspNetCore.Annotations;

using TClassificacaoTributaria;

namespace ReformaTributaria.API.Controllers;

[ApiController]
//[ApiKeyAuth]
[AllowAnonymous]
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
    public async Task<OkObjectResult> PostData([FromBody] List<ClassificacaoTributariaModel> data)
    {
        logger.LogInformation("Dados recebidos: {DataCount}", data.Count);
        return Ok(await reformaTributariaService.InsereDados(data));
    }

    [HttpGet]
    [SwaggerOperation("Lista tabela de Classificações Tributárias", "")]
    [Route("")]
    public async Task<List<ReformaTributariaListModel>> GetData()
    {
        return await reformaTributariaService.GetDados();
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
    public async Task<List<AnexoListModel>> GetDataAnexos()
    {
        return await reformaTributariaService.GetDadosAnexos();
    }
}