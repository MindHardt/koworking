using Bogus;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Koworking.Api.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Koworking.Api.Features.Vacancies.Actions;

[Handler, MapGet("vacancies")]
public static partial class GetVacancies
{
    public record Request : Paginated.Request;

    private static async ValueTask<Ok<Paginated.Response<Vacancy.Model>>> HandleAsync(
        Request request,
        Faker faker,
        CancellationToken ct)
    {
        var results = Enumerable.Range(0, request.Limit ?? 10)
            .Select(_ => new Vacancy.Model
            {
                Name = faker.Name.JobTitle(),
                Location = $"{faker.Address.City()}, {faker.Address.Country()}",
                ImageUrl = faker.PickRandom(null, $"https://d2ph5fj80uercy.cloudfront.net/04/cat{Random.Shared.Next(1000)}.jpg")
            })
            .ToArray();

        return TypedResults.Ok(results.ToPaginatedResponse(request));
    }
}