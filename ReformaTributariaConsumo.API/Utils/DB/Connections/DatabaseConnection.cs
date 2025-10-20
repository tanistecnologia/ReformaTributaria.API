using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace Tanis.Utils.Lib.DB.Connections;

public enum ConnectStr
{
    dbTanis,
    dbFWC,
    dbGTWeb,
    dbPGGalago,
    dbDockerLocal,
    dbTanisWeb,
    dbPeixaria,
    dbHubLogin,

    //dbHubAdmin,
    dbHubCEP,
    dbHangfire,
    dbTanisHub,

    dbMercantis
}

public class ConnDB<T> where T : DbConnection, new()
{
    public static T? Get(string connectionString)
    {
        try
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Parâmetro ConnectionString não pode estar vazio",
                    nameof(connectionString));

            T databaseConnection = new()
            {
                ConnectionString = connectionString
            };

            if (databaseConnection.State != ConnectionState.Closed) return databaseConnection;

            databaseConnection.Disposed +=
                (_, _) => Console.WriteLine("Fechando a conexao...");

            databaseConnection.Open();
            stopwatch.Stop();
            Console.WriteLine(
                $"Conectando em {databaseConnection.Database.ToLower()} / V.{databaseConnection.ServerVersion} - {stopwatch.ElapsedMilliseconds} ms");

            return databaseConnection;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"StrConn: {connectionString} - State: {ex.Message}");
            return null;
        }
    }

    public static T? GetNotOpen(string connectionString)
    {
        try
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Parâmetro ConnectionString não pode estar vazio",
                    nameof(connectionString));

            T sqlConnection = new()
            {
                ConnectionString = connectionString
            };

            return sqlConnection;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"StrConn: {connectionString} - State: {ex.Message}");
            return null;
        }
    }

    public static async Task<T?> GetAsync(string connectionString)
    {
        try
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Parâmetro ConnectionString não pode estar vazio",
                    nameof(connectionString));

            T sqlConnection = new()
            {
                ConnectionString = connectionString
            };

            if (sqlConnection.State == ConnectionState.Closed)
            {
                await sqlConnection.OpenAsync();
                //Console.WriteLine($"StrConn: {ConnectionString} - State: {sqlConnection.State}");                   
            }

            return sqlConnection;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"StrConn: {connectionString} - State: {ex.Message}");
            return null;
        }
    }
}

public static class ConnStr
{
    public static string Get(ConnectStr strCon)
    {
        var dev = string.Empty;
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ||
            Debugger.IsAttached)
            dev = "DEV_";

        var conf = strCon switch
        {
            ConnectStr.dbGTWeb => $"ConnectionStrings:{dev}DBWeb",
            ConnectStr.dbFWC => $"ConnectionStrings:{dev}FWC",
            ConnectStr.dbTanis => $"ConnectionStrings:{dev}DBMercantis",
            ConnectStr.dbDockerLocal => $"ConnectionStrings:{dev}DOCKER_LOCAL",
            ConnectStr.dbTanisWeb => $"ConnectionStrings:{dev}TanisWeb",
            ConnectStr.dbPeixaria => $"ConnectionStrings:{dev}PEIXARIA",
            ConnectStr.dbHubLogin => $"ConnectionStrings:{dev}DB_LOGIN",
            //ConnectStr.dbHubAdmin => $"ConnectionStrings:{dev}DB_ADMIN",
            ConnectStr.dbHubCEP => $"ConnectionStrings:{dev}DB_CEP",
            ConnectStr.dbPGGalago => $"ConnectionStrings:{dev}DB_GALAGO",
            ConnectStr.dbHangfire => $"ConnectionStrings:{dev}DB_HANGFIRE",
            ConnectStr.dbTanisHub => $"ConnectionStrings:{dev}DB_TANISHUB",
            ConnectStr.dbMercantis => $"ConnectionStrings:{dev}DB_MERCANTIS",
            _ => ""
        };

        var dir = Directory.GetCurrentDirectory();
        var builder = new ConfigurationBuilder().SetBasePath(dir).AddJsonFile("appsettings.json");
        var configuration = builder.Build();
        return configuration[conf] ?? "";
    }
}