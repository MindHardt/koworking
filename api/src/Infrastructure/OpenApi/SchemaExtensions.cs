using Microsoft.OpenApi.Models;

namespace Koworking.Api.Infrastructure.OpenApi;

public static class SchemaExtensions
{
    public static void CopyTo(this OpenApiSchema source, OpenApiSchema target)
    {
        target.Title = source.Title;
        target.Type = source.Type;
        target.Format = source.Format;
        target.Description = source.Description;
        target.Maximum = source.Maximum;
        target.ExclusiveMaximum = source.ExclusiveMaximum;
        target.Minimum = source.Minimum;
        target.ExclusiveMinimum = source.ExclusiveMinimum;
        target.MaxLength = source.MaxLength;
        target.MinLength = source.MinLength;
        target.Pattern = source.Pattern;
        target.MultipleOf = source.MultipleOf;
        target.Default = source.Default;
        target.ReadOnly = source.ReadOnly;
        target.WriteOnly = source.WriteOnly;
        target.AllOf = source.AllOf;
        target.OneOf = source.OneOf;
        target.AnyOf = source.AnyOf;
        target.Not = source.Not;
        target.Required = source.Required;
        target.Items = source.Items;
        target.MaxItems = source.MaxItems;
        target.MinItems = source.MinItems;
        target.UniqueItems = source.UniqueItems;
        target.Properties = source.Properties;
        target.MaxProperties = source.MaxProperties;
        target.MinProperties = source.MinProperties;
        target.AdditionalPropertiesAllowed = source.AdditionalPropertiesAllowed;
        target.AdditionalProperties = source.AdditionalProperties;
        target.Discriminator = source.Discriminator;
        target.Example = source.Example;
        target.Enum = source.Enum;
        target.Nullable = source.Nullable;
        target.ExternalDocs = source.ExternalDocs;
        target.Deprecated = source.Deprecated;
        target.Xml = source.Xml;
        target.Extensions = source.Extensions;
        target.UnresolvedReference = source.UnresolvedReference;
        target.Reference = source.Reference;
        target.Annotations = source.Annotations;
    }
}