using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;

using ReformaTributaria.API.Services;
using ReformaTributaria.API.Utils;

using Swashbuckle.AspNetCore.SwaggerUI;

using System.Data;
using System.Reflection;

using Tanis.Utils.Lib.DB.Connections;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddResponseCompression();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddSession();

builder.Services.AddScoped<ReformaTributariaService>();
builder.Services.AddScoped<IDbConnection>(_ => ConnDB<SqlConnection>.Get(ConnStr.Get(ConnectStr.dbMercantis))!);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = new LowerCaseNamingPolicy(); });

builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

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
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };

    var requirement = new OpenApiSecurityRequirement
    {
        { scheme, new List<string>() }
    };

    c.AddSecurityRequirement(requirement);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{ }


app.UseSwagger(options => options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Reforma Tributária do Consumo - Tanis Hub");
    c.DocumentTitle = "API Reforma Tributária do Consumo - Tanis Hub";
    c.RoutePrefix = "swagger";
    c.DocExpansion(DocExpansion.None);
    c.DisplayRequestDuration();
});


app.UseApiKeyAuth();

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();