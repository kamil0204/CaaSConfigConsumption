using CaaSSample.API.Services;
using CaaSSample.API.Worker;
using CaaSSample.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region ConfigMap configs import

if (builder.Configuration["DOTNET_ENVIRONMENT"] == null)
{
    string configMapDirectory = builder.Configuration["ConfigMapMount"]!;
    Console.WriteLine($"Initial Config Read");
    if (Directory.Exists(configMapDirectory))
        foreach (var filePath in Directory.GetFiles(configMapDirectory))
        {
            string key = Path.GetFileName(filePath);
            string value = File.ReadAllText(filePath);
            File.Delete(filePath);
            Environment.SetEnvironmentVariable(key, value);
        }

    string secretMapDirectory = builder.Configuration["SecretMapMount"]!;
    Console.WriteLine($"Initial Secret Read");
    if (Directory.Exists(configMapDirectory))
        foreach (var filePath in Directory.GetFiles(configMapDirectory))
        {
            string key = Path.GetFileName(filePath);
            string value = File.ReadAllText(filePath);
            Environment.SetEnvironmentVariable(key, value);
            File.Delete(filePath);
        }

    builder.Services.AddHostedService<ConfigWatcherWorker>();
}

#endregion

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IDBConfig, IFSDBConfig>();

IConfiguration configuration = new ConfigurationBuilder()
            .Build();

builder.Services.Configure<DBOptions>(builder.Configuration.GetSection("ConnectionStrings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
