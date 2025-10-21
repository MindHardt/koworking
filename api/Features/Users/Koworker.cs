using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Riok.Mapperly.Abstractions;

namespace Koworking.Api.Features.Users;

public partial record Koworker
{
    public long Id { get; set; }
    [MapperIgnore]
    public required Guid KeycloakId { get; set; }

    public class EntityConfiguration : IEntityTypeConfiguration<Koworker>
    {
        public void Configure(EntityTypeBuilder<Koworker> builder)
        {
            builder.HasIndex(x => x.KeycloakId).IsUnique();
        }
    }
}