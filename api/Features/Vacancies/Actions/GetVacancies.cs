using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Koworking.Api.Infrastructure;
using Koworking.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Koworking.Api.Features.Vacancies.Actions;

[Handler, MapGet("vacancies")]
public static partial class GetVacancies
{
    public record Request : Paginated.Request
    {
        public string? Search { get; set; }
    }

    private static async ValueTask<Ok<Paginated.Response<Vacancy.Model>>> HandleAsync(
        Request request,
        DataContext dataContext,
        Vacancy.Mapper mapper,
        CancellationToken ct)
    {
        var query = dataContext.Vacancies.AsNoTracking();
        if (string.IsNullOrWhiteSpace(request.Search) is false)
        {
            query = query.Where(x => EF.Functions.ILike(
                x.Title + " " + x.Text,
                $"%{request.Search}%"));
        }
        
        return TypedResults.Ok(await query
            .OrderBy(x => x.Id)
            .Project(mapper.ProjectToModels)
            .ToPaginatedResponseAsync(request, ct));
    }
}