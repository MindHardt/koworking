using Bogus;
using Koworking.Api;
using Koworking.Api.Infrastructure;
using Koworking.Api.Infrastructure.Data;
using Koworking.Api.Infrastructure.OpenApi;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(sp =>
    new NpgsqlDataSourceBuilder(sp.GetRequiredService<IConfiguration>().GetConnectionString("Postgres"))
        .EnableDynamicJson()
        .Build());
builder.Services.AddDbContext<DataContext>((sp, ef) =>
{
    ef.UseNpgsql(sp.GetRequiredService<NpgsqlDataSource>());
});

builder.Services.AddOpenApi(openApi =>
{
    openApi.AddSchemaTransformer<SchemaNamingTransformer>();
    openApi.AddSchemaTransformer<ValueObjectTransformer>();
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
}

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction() is false)
{
    app.MapOpenApi();
    app.MapScalarApiReference("/scalar");
    app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
}

app.MapApiEndpoints();

app.Run();
