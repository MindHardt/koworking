using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koworking.Api.Infrastructure.Data.Analytics;

public static class SnakeCaseTable
{
    private static readonly SnakeCaseNameRewriter Rewriter = new(CultureInfo.InvariantCulture);
    public static string ToSnakeCase(this string name) => Rewriter.RewriteName(name);
    
    public static EntityTypeBuilder<T> ToSnakeCaseTable<T>(
        this EntityTypeBuilder<T> builder,
        Action<TableBuilder<T>>? action = null) where T : class
        => builder.ToTable(table =>
        {
            table.Metadata.SetTableName(table.Metadata.GetTableName()!.ToSnakeCase());
            foreach (var prop in table.Metadata.GetProperties())
            {
                prop.SetColumnName(prop.Name.ToSnakeCase());
            }
            action?.Invoke(table);
        });
}