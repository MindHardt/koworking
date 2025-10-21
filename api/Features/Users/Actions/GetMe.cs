using System.Security.Claims;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Koworking.Api.Infrastructure;
using Koworking.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Koworking.Api.Features.Users.Actions;

[Handler, MapGet("/koworkers/me")]
public static partial class GetMe
{
    public record Request
    {
        public required HttpContext Context { get; set; }
    }
    
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .WithTags(nameof(Koworker))
        .WithDescription("Получение данных текущего пользователя");

    private static async ValueTask<Results<UnauthorizedHttpResult, Ok<Koworker.Model>>> HandleAsync(
        Request request,
        DataContext dataContext,
        Koworker.Mapper mapper,
        CancellationToken ct)
    {
        if (request.Context.User.FindFirstValue(ClaimTypes.NameIdentifier) is not { } userId ||
            Guid.TryParse(userId, out var kcId) is false)
        {
            return TypedResults.Unauthorized();
        }

        var koworker = await dataContext.Koworkers
            .Where(x => x.KeycloakId == kcId)
            .Project(mapper.ProjectToModels)
            .FirstOrDefaultAsync(ct);
        if (koworker is not null)
        {
            return TypedResults.Ok(koworker);
        }
        
        var newKoworker = new Koworker
        {
            KeycloakId = kcId,
        };
        dataContext.Koworkers.Add(newKoworker);
        await dataContext.SaveChangesAsync(ct);
        
        return TypedResults.Ok(mapper.ToModel(newKoworker));
    }
}