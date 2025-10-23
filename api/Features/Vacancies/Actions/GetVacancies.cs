using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Koworking.Api.Infrastructure;
using Koworking.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Koworking.Api.Features.Vacancies.Actions;

[Handler, MapGet("/vacancies/")]
public static partial class GetVacancies
{
    public record Request : Paginated.Request
    {
        public string? Search { get; set; }
    }

    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .WithTags(nameof(Vacancy))
        .WithDescription("Получение списка вакансий с поиском");

    private static async ValueTask<Ok<Paginated.Response<Vacancy.Model>>> HandleAsync(
        Request request,
        DataContext dataContext,
        Vacancy.Mapper mapper,
        CancellationToken ct)
    {
        var query = dataContext.Vacancies.AsNoTracking();
        
        if (string.IsNullOrWhiteSpace(request.Search) is false)
        {
            var search = BuildTsQuery(request.Search);
            query = query
                // ReSharper disable once EntityFramework.UnsupportedServerSideFunctionCall
                .Where(x => x.TsVector.Rank(EF.Functions.ToTsQuery(BuildTsQuery(request.Search))) > 0)
                // ReSharper disable once EntityFramework.UnsupportedServerSideFunctionCall
                .OrderByDescending(x => x.TsVector.Rank(EF.Functions.ToTsQuery(BuildTsQuery(request.Search))))
                .ThenByDescending(x => x.CreatedAt);
        }
        else
        {
            query = query.OrderByDescending(x => x.CreatedAt);
        }
        
        return TypedResults.Ok(await query
            .Project(mapper.ProjectToModels)
            .ToPaginatedResponseAsync(request, ct));
    }

    private static string BuildTsQuery(string search) => string.Join(" <-> ", search
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(x => $"{x}:*"));
}