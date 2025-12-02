using AspNetCore.Scalar;

using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.PostgreSql;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi;

using Npgsql;

using ReformaTributaria.API.Services;
using ReformaTributaria.API.Services.Middlewares;
using ReformaTributaria.API.Utils.DB.HealthChecks;
using ReformaTributaria.API.Utils.Json;

using Swashbuckle.AspNetCore.SwaggerUI;

using System.Data;
using System.Reflection;

using Tanis.Utils.Lib.DB.Connections;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(config =>
    config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseColouredConsoleLogProvider()
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(configure: configure =>
        {
            configure.UseNpgsqlConnection(ConnStr.Get(ConnectStr.dbHangfire));
        }));

builder.Services.AddHangfireServer();

builder.Services
    .AddHealthChecks()
    .Add(new HealthCheckRegistration(
        "SQL Server",
        sp => new DatabaseHealthCheck(sp.GetRequiredKeyedService<IDbConnection>("SQLServer")),
        failureStatus: HealthStatus.Unhealthy,
        tags: null
    ))
    .Add(new HealthCheckRegistration(
        "PostgreSQL",
        sp => new DatabaseHealthCheck(sp.GetRequiredKeyedService<IDbConnection>("PostgreSQL")),
        failureStatus: HealthStatus.Unhealthy,
        tags: null
    ));

builder.Services.AddResponseCompression();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddScoped<ReformaTributariaService>();
builder.Services.AddKeyedScoped<IDbConnection>(
    "SQLServer",
    (_, _) => ConnDB<SqlConnection>.Get(ConnStr.Get(ConnectStr.dbMercantis))!);
builder.Services.AddKeyedScoped<IDbConnection>(
    "PostgreSQL",
    (_, _) => ConnDB<NpgsqlConnection>.Get(ConnStr.Get(ConnectStr.dbHangfire))!);

// builder.Services.AddMvc(config =>
// {
//     var policy = new AuthorizationPolicyBuilder()
//         .RequireAuthenticatedUser()
//         .Build();
//     config.Filters.Add(new AuthorizeFilter(policy));
// });
//
// builder.Services.AddAuthorizationBuilder()
//     .AddPolicy("user", policy => policy.RequireClaim("Store", "user"))
//     .AddPolicy("admin", policy => policy.RequireClaim("Store", "admin"));

builder.Services.AddMvc();

builder.Services
    .AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = new LowerCaseNamingPolicy(); });

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(configurePolicy =>
        configurePolicy
            .AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod()
    ));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = $"Reforma Tributária do Consumo API (V.{Assembly.GetEntryAssembly()?.GetName().Version?.ToString()})",
        Version = "v1",
        Description = @"### Autenticação
                    Todas as requisições devem incluir o header 'x-api-key'.
                    
                    ### Códigos de Erro
                    - 401: API Key não fornecida ou inválida
                    - 403: API Key sem permissão para o recurso"
    });

    // Configuração da API Key no Swagger
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Insira sua API key no header",
        Type = SecuritySchemeType.ApiKey,
        Name = "x-api-key", // Nome do header
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    // Requisito de segurança global
    //var scheme = new OpenApiSecurityScheme
    //{

    //    Reference = new OpenApiReference
    //    {
    //        Type = ReferenceType.SecurityScheme,
    //        Id = "ApiKey"
    //    },
    //    In = ParameterLocation.Header
    //};

    //var requirement = new OpenApiSecurityRequirement
    //{
    //    { scheme, new List<string>() }
    //};

    c.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("ApiKey", doc)] = []
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger(options => options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Reforma Tributária do Consumo - Tanis Hub");
    c.DocumentTitle = "API Reforma Tributária do Consumo - Tanis Hub";
    c.RoutePrefix = "swagger";
    c.DocExpansion(DocExpansion.None);
    c.DisplayRequestDuration();
});

app.UseScalar(options =>
{
    options.UseTheme(Theme.Default);
    options.RoutePrefix = "api-docs";
});

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();

//app.UseAuthorization();
app.UseApiKeyAuth();

app.MapControllers();

var options = new DashboardOptions
{
    Authorization =
    [
        new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
        {
            SslRedirect = false,
            RequireSsl = false,
            LoginCaseSensitive = true,
            Users =
            [
                new BasicAuthAuthorizationUser
                {
                    Login = "hangfire",
                    PasswordClear = "lmi1970--"
                }
            ]
        })
    ]
};
app.UseHangfireDashboard("/hangfire", options);

app.UseHealthChecks(
    path: "/healthz",
    options: new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();