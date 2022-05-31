using DogsCatalog.Repositories;
using DogsCatalog.Settings;
using System.Configuration;
using System.Collections.Generic;
using System.Text.Json;
using System.Net.Mime;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using DogsCatalog;

var builder = WebApplication.CreateBuilder(args);
var mongoDbsettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();


// Add services to the container.
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

//service for health DB cheсk
builder.Services.AddHealthChecks()
.AddMongoDb(
    mongoDbsettings.ConnectionString,
    name: "mongodb",
    timeout: TimeSpan.FromSeconds(3),
    tags: new[] { "ready" });

//services for configuration and singletons
builder.Services.Configure<MongoDatabaseSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));
builder.Services.AddSingleton<MongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
//builder.Services.AddSingleton<IMongoClient>(s=> new MongoClient(builder.Configuration.GetValue<string>("MongoDbSettings : ConnectionString")));

builder.Services.AddSingleton<IMongoClient>(ServiceProvider =>
{

    var settings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    return new MongoClient(settings.ConnectionString);
});
builder.Services.AddSingleton<IDogsRepository, MongoDbDogsRepository>();

//builder.Services.AddScoped<IItemsRepository, InMemItemsRepository>();




builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        var result = JsonSerializer.Serialize(
            new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                    duration = entry.Value.Duration.ToString()
                })
            }
        );

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = (_) => false
});

app.Run();
