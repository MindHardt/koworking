using Bogus;
using Koworking.Api;
using Koworking.Api.Infrastructure.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(openApi =>
{
    openApi.AddSchemaTransformer<SchemaNamingTransformer>();
    openApi.CreateSchemaReferenceId = type => SchemaNamingTransformer.GetTypeName(type.Type);
});
builder.Services.AddSingleton<Faker>(_ => new Faker("ru"));
builder.Services.AddApiHandlers();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction() is false)
{
    app.MapOpenApi();
    app.MapScalarApiReference("/scalar");
    app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
}

app.MapApiEndpoints();

app.Run();
