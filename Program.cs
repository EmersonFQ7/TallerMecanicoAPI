
using Microsoft.EntityFrameworkCore;
using TallerMecanicoAPI.Data;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddOpenApi();

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
