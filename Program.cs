using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using ElasticSearch.Middelware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

ConfigureLogging(
    builder.Configuration,
    builder.Environment.EnvironmentName);

builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddelware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


void ConfigureLogging(
    IConfiguration configuration,
    string environment)
{
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .Enrich.WithProperty("Environment", environment)
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(
            ConfigureElasticSink(configuration, environment))
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(
    IConfiguration configuration,
    string environment)
{
    var uri = configuration["ElasticConfiguration:Uri"]
        ?? throw new InvalidOperationException(
            "Elasticsearch URI is missing");

    return new ElasticsearchSinkOptions(new Uri(uri))
    {
        AutoRegisterTemplate = true,
        IndexFormat =
            $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLowerInvariant().Replace(".", "-")}-{environment.ToLowerInvariant()}-{DateTime.UtcNow:yyyy-MM}",
        NumberOfReplicas = 1,
        NumberOfShards = 2
    };
}