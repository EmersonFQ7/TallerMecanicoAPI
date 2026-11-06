using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using TallerMecanicoAPI.Data;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configurar encabezados enviados por el proxy de Render
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Controladores
builder.Services.AddControllers();

// Seleccionar base de datos según el entorno
builder.Services.AddDbContext<TallerMecanicoContext>(options =>
{
    var connectionString = builder.Configuration
        .GetConnectionString("TallerMecanicoConnection");

    if (builder.Environment.IsProduction())
    {
        // PostgreSQL en Neon (Render)
        options.UseNpgsql(connectionString);
    }
    else
    {
        // SQL Server local
        options.UseSqlServer(connectionString);
    }
});

// OpenAPI y Swagger
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendLocal", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseForwardedHeaders();

// OpenAPI
app.MapOpenApi();

// Swagger UI
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "TallerMecanicoAPI v1");
});

app.UseHttpsRedirection();

app.UseCors("FrontendLocal");

app.UseAuthorization();

app.MapControllers();

app.Run();