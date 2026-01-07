using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;
using WealthVaultApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig
        .MinimumLevel.Information()
        .WriteTo.Console()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "WealthVaultApi")
);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        // Serialize/deserialize enums as their string names in JSON
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "Wealth Vault API", Version = "v1" }));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var allKeys = builder.Configuration.AsEnumerable().Select(x => x.Key);
Console.WriteLine("All config keys: " + string.Join(", ", allKeys));

var connectionString = builder.Configuration.GetConnectionString("WealthVaultDBConnString")
                    ?? builder.Configuration["POSTGRESQLCONNSTR_WealthVaultDBConnString"]
                    ?? throw new InvalidOperationException("Database connection string not found!");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));  // Stub; swap UseSqlServer for Azure

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();


app.Run();
