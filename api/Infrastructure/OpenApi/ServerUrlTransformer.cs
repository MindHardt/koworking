using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Koworking.Api.Infrastructure.OpenApi;

public class ServerUrlTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument doc, OpenApiDocumentTransformerContext ctx, CancellationToken ct)
    {
        var config = ctx.ApplicationServices.GetRequiredService<IConfiguration>();
        if (config.GetSection("OpenApi:Servers").Get<string[]>() is [_, ..] servers)
        {
            doc.Servers = servers.Select(x => new OpenApiServer { Url = x }).ToList();
        }
        return Task.CompletedTask;
    }
}