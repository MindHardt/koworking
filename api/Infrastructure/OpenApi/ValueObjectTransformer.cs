using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Vogen;

namespace Koworking.Api.Infrastructure.OpenApi;

public class ValueObjectTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext ctx, CancellationToken ct)
    {
        var schemaType = ctx.JsonTypeInfo.Type;
        if (schemaType.IsGenericType && schemaType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            schemaType = schemaType.GetGenericArguments()[0];
        }
        
        if (schemaType.GetCustomAttribute<DefaultValueAttribute>() is { Value: { } defaultValue })
        {
            schema.Example = defaultValue switch
            {
                string s => new OpenApiString(s),
                int i => new OpenApiInteger(i),
                decimal d => new OpenApiDouble((double)d),
                double d => new OpenApiDouble(d),
                _ => throw new InvalidOperationException(
                    $"Cannot add schema example for type {schemaType}, value is of type {defaultValue.GetType()}")
            };
        }
        
        var voAttribute = schemaType.GetCustomAttribute<ValueObjectAttribute>();
        if (voAttribute is null)
        {
            return Task.CompletedTask;
        }
        
        var voType = voAttribute.GetType().GetGenericArguments()[0];
        var defaultSchema = voType.MapTypeToOpenApiPrimitiveType();
        
        (schema.Type, schema.Format) = (defaultSchema.Type, defaultSchema.Format);
        
        return Task.CompletedTask;
    }
}