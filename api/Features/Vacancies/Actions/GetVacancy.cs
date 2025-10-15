using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Koworking.Api.Infrastructure;
using Koworking.Api.Infrastructure.Data;
using Koworking.Api.Infrastructure.TextIds;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Koworking.Api.Features.Vacancies.Actions;

[Handler, MapGet($"/vacancies/{{{nameof(Request.Id)}}}")]
public static partial class GetVacancy
{
    public record Request
    {
        [FromRoute]
        public required TextId Id { get; set; }
    }
    
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .WithTags(nameof(Vacancy))
        .WithDescription("Получение конкретной вакансии");

    private static async ValueTask<Results<Ok<Vacancy.Model>, NotFound>> HandleAsync(
        Request request,
        DataContext dataContext,
        Vacancy.Mapper mapper,
        CancellationToken ct)
    {
        if (mapper.Encoder.DecodeTextId(request.Id) is not { } id)
        {
            return TypedResults.NotFound();
        }

        var vacancy = await dataContext.Vacancies
            .Where(x => x.Id == id)
            .AsNoTracking()
            .Project(mapper.ProjectToModels)
            .FirstOrDefaultAsync(ct);
        return vacancy is null ? TypedResults.NotFound() : TypedResults.Ok(vacancy);
    }
}