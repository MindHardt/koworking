using Koworking.Api.Infrastructure.TextIds;

namespace Koworking.Api.Features.Vacancies;

public partial record Vacancy 
{
    public record Info
    {
        public required string Title { get; set; }
        public required string Location { get; set; }
        public required string Text { get; set; }
        public required string? ImageUrl { get; set; }
        public required Paycheck? Paycheck { get; set; }
    }
    
    public record Model : Info
    {
        public required TextId Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required DateTimeOffset UpdatedAt { get; set; }
    }
}