using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Koworking.Api.Infrastructure.OpenApi;

public class SecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument doc, OpenApiDocumentTransformerContext ctx, CancellationToken ct)
    {
        doc.Components ??= new OpenApiComponents();
        doc.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();
        doc.Components.SecuritySchemes["Keycloak"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "jwt",
            Description = "Keycloak Bearer Token"
        };
        if (ctx.ApplicationServices.GetRequiredService<IHostEnvironment>().IsDevelopment())
        {
            doc.Components.SecuritySchemes["Fake"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Description = "Fake auth for tests",
                Name = "x-user-id",
                In = ParameterLocation.Header,
                Scheme = "Fake",
            };
        }

        return Task.CompletedTask;
    }
}