using Bogus;
using Koworking.Api;
using Koworking.Api.Features.Uploads;
using Koworking.Api.Infrastructure;
using Koworking.Api.Infrastructure.Data;
using Koworking.Api.Infrastructure.OpenApi;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using Scalar.AspNetCore;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog(logger => logger.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddSingleton(sp =>
    new NpgsqlDataSourceBuilder(sp.GetRequiredService<IConfiguration>().GetConnectionString("Postgres"))
        .EnableDynamicJson()
        .Build());
builder.Services.AddDbContext<DataContext>((sp, ef) =>
{
    ef.UseNpgsql(sp.GetRequiredService<NpgsqlDataSource>());
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwt =>
{
    jwt.Authority = builder.Configuration["Jwt:Authority"];
    jwt.Audience = builder.Configuration["Jwt:Audience"];
    jwt.RequireHttpsMetadata = builder.Environment.IsProduction();
});
builder.Services.AddDataProtection().PersistKeysToDbContext<DataContext>();
builder.Services.AddAuthorization();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddFakeAuth();
}

builder.Services.AddS3FileStorage();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddHybridCache();
if (builder.Configuration.GetConnectionString("Redis") is { Length: > 0 } redisConnection)
{
    builder.Services.AddSingleton(sp => ConnectionMultiplexer.Connect(redisConnection, multiplexer =>
    {
        multiplexer.LoggerFactory = sp.GetRequiredService<ILoggerFactory>();
    }));
    builder.Services.AddStackExchangeRedisCache(redis =>
    {
        redis.Configuration = builder.Configuration["Redis:Configuration"];
    });
}
else
{
    builder.Services.AddDistributedMemoryCache();
}

builder.Services.AddOpenApi(openApi =>
{
    openApi.AddSchemaTransformer<SchemaNamingTransformer>();
    openApi.AddSchemaTransformer<ValueObjectTransformer>();
    openApi.AddDocumentTransformer<SecuritySchemeTransformer>();
    openApi.AddDocumentTransformer<SecuritySchemeTransformer>();
    openApi.CreateSchemaReferenceId = type => SchemaNamingTransformer.GetTypeName(type.Type);
});
builder.Services.ConfigureHttpJsonOptions(httpJson => httpJson.SerializerOptions.SetDefaults());
builder.Services.AutoRegisterFromApi();
builder.Services.AddSingleton<Faker>(_ => new Faker("ru"));
builder.Services.AddApiHandlers();
builder.Services.AddCors();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetRequiredService<DataContext>().Database.MigrateAsync();
    if (builder.Environment.IsProduction() is false)
    {
        await scope.ServiceProvider.GetRequiredService<S3FileStorage>().Initialize();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction() is false)
{
    app.MapOpenApi();
    app.MapScalarApiReference("/scalar", scalar => scalar.AddPreferredSecuritySchemes(JwtBearerDefaults.AuthenticationScheme));
    app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapApiEndpoints();

app.Run();