using Koworking.Api.Infrastructure.TextIds;

namespace Koworking.Api.Features.Users;

public partial record Koworker
{
    public record Model
    {
        public required TextId Id { get; set; }
    }
}