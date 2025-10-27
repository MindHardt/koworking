using Koworking.Api.Infrastructure.Data.Analytics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koworking.Api.Features.Visits;

public partial record SiteVisit
{
    public required string UtmSource { get; set; }
    public required string UtmMedium { get; set; }
    public required string UtmCampaign { get; set; }
    public string? UtmTerm { get; set; }
    public string? UtmContent { get; set; }
    // https://clickhouse.com/docs/integrations/csharp#supported-data-types
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public class EntityConfiguration : IEntityTypeConfiguration<SiteVisit>
    {
        public void Configure(EntityTypeBuilder<SiteVisit> builder)
        {
            builder.HasNoKey();
            builder.ToSnakeCaseTable(x => x
                .HasMergeTreeEngine()
                .WithOrderBy(nameof(CreatedAt).ToSnakeCase()));
        }
    }
}