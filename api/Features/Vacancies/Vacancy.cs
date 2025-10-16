using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;
using Riok.Mapperly.Abstractions;

namespace Koworking.Api.Features.Vacancies;

public partial record Vacancy
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required string Location { get; set; }
    public required string Text { get; set; }
    public required string? ImageUrl { get; set; }
    public required Paycheck? Paycheck { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    [MapperIgnore]
    public NpgsqlTsVector TsVector { get; set; } = null!;
    
    public class EntityConfiguration : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> builder)
        {
            builder.OwnsOne(x => x.Paycheck);
            builder.Property(x => x.TsVector)
                .HasComputedColumnSql("gen_vacancy_vector(title, text)", stored: true);
            builder.HasIndex(x => x.TsVector).HasMethod("GIN");
        }
    }
}

public record Paycheck
{
    public required int Amount { get; set; }
    public required PaycheckPeriod Period { get; set; }
    public required PaycheckType Type { get; set; }
}

public enum PaycheckPeriod : sbyte
{
    Hourly = 1,
    Daily = 2,
    Weekly = 3,
    Monthly = 4,
    Yearly = 5
}

public enum PaycheckType : sbyte
{
    Net = 1,
    Gross = 2
}