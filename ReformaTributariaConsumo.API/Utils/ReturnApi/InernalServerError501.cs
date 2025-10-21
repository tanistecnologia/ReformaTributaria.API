using Microsoft.AspNetCore.Mvc;

using Tanis.Utils.Lib.DTO;
using Tanis.Utils.Lib.ResultPattern.Model;

namespace Tanis.Utils.Lib.ReturnApi;

public static class InernalServerError501
{
    public static IActionResult E(string requestPath, string codigo, string descricao)
    {
        return new ObjectResult(
            new ResponseDTO<ResultErrorModel>(
                    requestPath,
                    new ResultErrorModel()
                    {
                        Codigo = codigo,
                        Descricao = descricao
                    })
            )
        {
            StatusCode = 501
        };
    }
    public static IActionResult E(string requestPath, ResultErrorModel error)
    {
        return new ObjectResult(new ResponseDTO<ResultErrorModel>(requestPath, error))
        {
            StatusCode = 501
        };
    }
}
