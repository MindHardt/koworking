using Koworking.Api.Features.Visits;
using Microsoft.EntityFrameworkCore;

namespace Koworking.Api.Infrastructure.Data.Analytics;

public class AnalyticsContext(DbContextOptions<AnalyticsContext> options) : DbContext(options)
{
    public DbSet<SiteVisit> SiteVisits => Set<SiteVisit>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromContextEntities(GetType());
        base.OnModelCreating(modelBuilder);
    }
}