using System.Net.Mime;
using Bogus;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Koworking.Api.Features.Uploads;
using Koworking.Api.Features.Uploads.Actions;
using Koworking.Api.Features.Vacancies;
using Koworking.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Koworking.Api.Features.Dev;

[Handler, MapPost("/dev/seed")]
public static partial class Seed
{
    public record Request
    {
        public required HttpContext HttpContext { get; set; }
        [FromQuery]
        public int? Vacancies { get; set; }
    }
    
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .WithTags(nameof(Seed))
        .WithDescription("Сидирование БД случайными вакансиями")
        .RequireAuthorization();

    private static async ValueTask<Ok> HandleAsync(
        [AsParameters] Request request,
        PostUpload.Handler uploadHandler,
        DataContext dataContext,
        Faker faker,
        CancellationToken ct)
    {
        await using var transaction = await dataContext.Database.BeginTransactionAsync(ct);
        await dataContext.Vacancies.Where(_ => true).ExecuteDeleteAsync(ct);

        var uploads = Directory.EnumerateFiles(Path.Combine("Features", "Dev", "SeedData"))
            .Select(x => new FileInfo(x))
            .Select(async file =>
            {
                await using var contentStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                var req = new PostUpload.Request
                {
                    File = new FormFile(contentStream, 0, contentStream.Length, nameof(PostUpload.Request.File),
                        file.Name)
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = MediaTypeNames.Image.Jpeg
                    },
                    Scope = UploadScope.Attachment
                };
                var result = (Ok<Upload.Model>)(await uploadHandler.HandleAsync(req, ct)).Result;
                return result.Value!.GetDownloadUri(request.HttpContext.Request.GetEncodedUrl());
            })
            .Select(x => x.Result)
            .ToArray();
        
        dataContext.Vacancies.AddRange(Enumerable.Range(0, request.Vacancies ?? 20)
            .Select(_ =>
            {
                var title = faker.Name.JobTitle();
                return new Vacancy
                {
                    Title = title,
                    Location = faker.Address.City(),
                    Description = faker.Lorem.Paragraph(),
                    Conditions = string.Join('\n', faker.MakeLazy(faker.Random.Int(2, 5), i => $"{i}. {faker.Lorem.Sentence()}")),
                    Expectations = string.Join('\n', faker.MakeLazy(faker.Random.Int(2, 5), i => $"{i}. {faker.Lorem.Sentence()}")),
                    ImageUrl = faker.PickRandom(uploads).ToString(),
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