using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;

namespace Koworking.Api.Infrastructure.OpenApi;

public class OptionalTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.JsonTypeInfo.Type is not { IsGenericType: true } type ||
            type.GetGenericTypeDefinition() != typeof(Optional<>))
        {
            return Task.CompletedTask;
        }
        
        type.GetGenericArguments()[0].MapTypeToOpenApiPrimitiveType().CopyTo(schema);
        schema.Required.Clear();

        return Task.CompletedTask;
    }
}