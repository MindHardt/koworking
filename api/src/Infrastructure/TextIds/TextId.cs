using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Vogen;

namespace Koworking.Api.Infrastructure.TextIds;

[ValueObject<string>]
[SuppressMessage("Usage", "AddNormalizeInputMethod:Value Objects can have a method that normalizes (sanitizes) input")]
[SuppressMessage("Usage", "AddValidationMethod:Value Objects can have validation")]
public readonly partial record struct TextId
{
    [GeneratedRegex("[A-Z-a-z0-9-_]+")]
    public static partial Regex ValidationRegex { get; }

    public static Task CustomizeOpenApiSchema(OpenApiSchema schema, OpenApiSchemaTransformerContext ctx, CancellationToken ct)
    {
        schema.Nullable = false;
        return Task.CompletedTask;
    }
}