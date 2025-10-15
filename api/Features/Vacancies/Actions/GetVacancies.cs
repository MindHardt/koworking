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
        
        // ReSharper disable once EntityFramework.UnsupportedServerSideFunctionCall
        query = string.IsNullOrWhiteSpace(request.Search) is false 
            ? query
                .OrderByDescending(x => EF.Functions.ILike(x.Title, $"%{request.Search}%"))
                .ThenByDescending(x => x.TsVector.Rank(EF.Functions.ToTsQuery(request.Search)))
            : query.OrderByDescending(x => x.Id);
        
        return TypedResults.Ok(await query
            .Project(mapper.ProjectToModels)
            .ToPaginatedResponseAsync(request, ct));
    }
}