using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ReformaTributaria.API.Utils.DTO;

public class ResponseDTO<T> where T : class, new()
{

    public ResponseDTO()
    {
        Info = new BasicInfo<T>();
    }

    public ResponseDTO(string funcao)
    {
        Info = new BasicInfo<T>(funcao);
    }

    public ResponseDTO(string funcao, T value)
    {
        Info = new BasicInfo<T>(funcao)
        {
            Dados = value
        };
    }

    public ResponseDTO(string funcao, T value, string erro = "")
    {
        Info = new BasicInfo<T>(funcao)
        {
            ErroMsg = erro,
            Dados = value
        };
    }

    public BasicInfo<T> Info { get; set; }
}

public class BasicInfo<T> where T : class, new()
{
    [JsonPropertyName("versao_api")]
    public string VersaoApi { get; set; }
    public DateTime DataGeracao { get; }
    public string Funcao { get; set; }
    public int Quantidade => (Dados is IList list) ? list.Count : 1;
    public int Pagina { get; set; }
    public int TotalPaginas { get; set; }
    public int TotalRegistros { get; set; }
    public bool HouveErro => !string.IsNullOrEmpty(ErroMsg);
    public string ErroMsg { get; set; }
    public T Dados { get; set; }

    public BasicInfo(string funcao)
    {
        VersaoApi = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0";
        DataGeracao = DateTime.Now;
        Funcao = funcao;
        TotalPaginas = 1;
        TotalRegistros = 1;
        Pagina = 1;
        ErroMsg = "";
        Dados = new T();
    }

    public BasicInfo()
    {
        VersaoApi = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0";
        DataGeracao = DateTime.Now;
        Funcao = "";
        TotalPaginas = 1;
        TotalRegistros = 1;
        Pagina = 1;
        ErroMsg = "";
        Dados = new T();
    }
}

public class BasicInfoResult
{
    public int RowsAffected { get; set; }
    public int? ChaveGerada { get; set; }
    public string Erro { get; set; }
    public bool Ok => RowsAffected > 0;

    public BasicInfoResult()
    {
        RowsAffected = 0;
        ChaveGerada = 0;
        Erro = "";
    }
}

public class BasicInfoResult<T>
{
    public int RowsAffected { get; set; }
    public T? ChaveGerada { get; set; }
    public string Erro { get; set; }
    public bool Ok => RowsAffected > 0;

    public BasicInfoResult()
    {
        RowsAffected = 0;
        Erro = "";        
    }
}