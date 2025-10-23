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
    public record Request;
    
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .WithTags(nameof(Koworker))
        .WithDescription("Получение данных текущего пользователя")
        .RequireAuthorization();

    private static async ValueTask<Ok<Koworker.Model>> HandleAsync(
        Request _,
        CallerContext caller,
        CancellationToken ct)
    {
        var state = await caller.GetRequiredStateAsync(ct);
        
        return TypedResults.Ok(state.User);
    }
}