using Koworking.Api.Features.Uploads;
using Koworking.Api.Features.Users;
using Koworking.Api.Features.Vacancies;
using Microsoft.EntityFrameworkCore;

namespace Koworking.Api.Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Vacancy> Vacancies => Set<Vacancy>();
    public DbSet<Koworker> Koworkers => Set<Koworker>();      
    public DbSet<Upload> Uploads => Set<Upload>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}