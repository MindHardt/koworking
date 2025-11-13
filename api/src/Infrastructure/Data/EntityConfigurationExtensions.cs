using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Koworking.Api.Infrastructure.Data;

public static class EntityConfigurationExtensions
{
    public static void ApplyConfigurationsFromContextEntities(this ModelBuilder model, Type contextType)
    {
        var configs = contextType
            .GetProperties()
            .Where(IsDbSetProp)
            .Select(x => x.PropertyType)
            .Select(GetEntityConfigType)
            .OfType<Type>();
        foreach (var configType in configs)
        {
            var entityType = configType.GetInterfaces().Single().GetGenericArguments()[0];
            var config = Activator.CreateInstance(configType);
            var apply = model.GetType()
                .GetMethod(nameof(model.ApplyConfiguration))!
                .MakeGenericMethod(entityType);
            apply.Invoke(model, [config]);
        }
    }
    
    private static bool IsDbSetProp(this PropertyInfo prop) => 
        prop.PropertyType is { IsGenericType: true } type && type.GetGenericTypeDefinition() == typeof(DbSet<>);

    private static Type? GetEntityConfigType(this Type dbSetType) => dbSetType
        .GetGenericArguments()[0]
        .GetNestedTypes()
        .Where(IsEntityConfigType)
        .FirstOrDefault();
    
    private static bool IsEntityConfigType(this Type type) => type.GetInterfaces()
        .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
}