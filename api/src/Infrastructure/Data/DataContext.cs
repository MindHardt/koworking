using Koworking.Api.Features.Uploads;
using Koworking.Api.Features.Users;
using Koworking.Api.Features.Vacancies;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Koworking.Api.Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IDataProtectionKeyContext
{
    public DbSet<Vacancy> Vacancies => Set<Vacancy>();
    public DbSet<Koworker> Koworkers => Set<Koworker>();      
    public DbSet<Upload> Uploads => Set<Upload>();
    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromContextEntities(GetType());
        base.OnModelCreating(modelBuilder);
    }
}