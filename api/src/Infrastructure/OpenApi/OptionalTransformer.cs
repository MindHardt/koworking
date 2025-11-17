using System.Reflection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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

        var nullCtx = new NullabilityInfoContext();
        var nullInfo = context switch
        {
            { JsonPropertyInfo.AttributeProvider: PropertyInfo prop }
                => nullCtx.Create(prop),
            { ParameterDescription.ParameterDescriptor: IPropertyInfoParameterDescriptor param }
                => nullCtx.Create(param.PropertyInfo),
            _ => throw new NotSupportedException()
        };

        schema.Nullable = nullInfo.GenericTypeArguments[0].WriteState == NullabilityState.Nullable;
        
        return Task.CompletedTask;
    }
}