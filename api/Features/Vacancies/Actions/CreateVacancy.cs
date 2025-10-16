using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Koworking.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Koworking.Api.Features.Vacancies.Actions;

[Handler, MapPost("/vacancies/")]
public static partial class CreateVacancy
{
    public record Request : Vacancy.Info;

    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .WithTags(nameof(Vacancy))
        .WithDescription("Создание новой вакансии");
    
    private static async ValueTask<Ok<Vacancy.Model>> HandleAsync(
        Request request,
        DataContext dataContext,
        Vacancy.Mapper mapper,
        CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;
        var vacancy = new Vacancy
        {
            Title = request.Title,
            Text = request.Text,
            ImageUrl = request.ImageUrl,
            Location = request.Location,
            Paycheck = request.Paycheck,
            CreatedAt = now,
            UpdatedAt = now
        };
        dataContext.Vacancies.Add(vacancy);
        await dataContext.SaveChangesAsync(ct);
        
        return TypedResults.Ok(mapper.ToModel(vacancy));
    }
}