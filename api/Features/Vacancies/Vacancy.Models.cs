using Koworking.Api.Infrastructure.TextIds;

namespace Koworking.Api.Features.Vacancies;

public partial record Vacancy 
{
    public record Model
    {
        public required TextId Id { get; set; }
        public required string Title { get; set; }
        public required string Location { get; set; }
        public required string Text { get; set; }
        public required string? ImageUrl { get; set; }
        public required Paycheck? Paycheck { get; set; }
    }
}