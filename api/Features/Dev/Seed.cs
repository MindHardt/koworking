using Bogus;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Koworking.Api.Features.Vacancies;
using Koworking.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Koworking.Api.Features.Dev;

[Handler, MapGet("/dev/seed")]
public static partial class Seed
{
    public record Request;

    private static async ValueTask<Ok> HandleAsync(
        Request _,
        DataContext dataContext,
        Faker faker,
        CancellationToken ct)
    {
        await using var transaction = await dataContext.Database.BeginTransactionAsync(ct);
        await dataContext.Vacancies.Where(_ => true).ExecuteDeleteAsync(ct);
        
        dataContext.Vacancies.AddRange(Enumerable.Range(0, 20)
            .Select(_ =>
            {
                var title = faker.Name.JobTitle();
                return new Vacancy
                {
                    Title = title,
                    Location = faker.Address.City(),
                    Text = faker.Lorem.Paragraphs(),
                    ImageUrl = faker.PickRandom(null, faker.Image.DataUri(640, 480, faker.Random.Hexadecimal(6, "#"))),
                    Paycheck = faker.PickRandom(null, new Paycheck
                    {
                        Amount = faker.Random.Int(20, 200) * 1000,
                        Period = faker.Random.Enum<PaycheckPeriod>(),
                        Type = faker.Random.Enum<PaycheckType>()
                    })
                };
            }));
        await dataContext.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        return TypedResults.Ok();
    }
}