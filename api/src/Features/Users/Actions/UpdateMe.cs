using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Koworking.Api.Infrastructure;
using Koworking.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Koworking.Api.Features.Users.Actions;

[Handler, MapPatch("/koworkers/me")]
public static partial class UpdateMe
{
    public record Request
    {
        public Optional<string?> AvatarUrl { get; set; }
    }
    
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .WithTags(nameof(Koworker))
        .WithDescription("Обновление текущего пользователя")
        .RequireAuthorization();

    private static async ValueTask<Ok<Koworker.Model>> HandleAsync(
        [FromBody] Request request,
        CallerContext caller,
        DataContext dataContext,
        Koworker.Mapper mapper,
        HybridCache cache,
        CancellationToken ct)
    {
        var userId = await caller.GetRequiredUserId(ct);
        var koworker = await dataContext.Koworkers.FirstAsync(x => x.Id == userId, ct);

        koworker.AvatarUrl = request.AvatarUrl.Or(koworker.AvatarUrl);
        
        await dataContext.SaveChangesAsync(ct);
        await cache.RemoveAsync(koworker.GetCacheKey(), ct);

        return TypedResults.Ok(mapper.ToModel(koworker));
    }
}