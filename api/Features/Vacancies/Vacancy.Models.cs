namespace Koworking.Api.Features.Vacancies;

public partial record Vacancy 
{
    public record Model
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public string? ImageUrl { get; set; }
    }
}