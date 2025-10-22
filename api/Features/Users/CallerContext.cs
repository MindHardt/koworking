using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Koworking.Api.Infrastructure;
using Koworking.Api.Infrastructure.Data;
using Koworking.Api.Infrastructure.TextIds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Koworking.Api.Features.Users;

[RegisterScoped]
public class CallerContext(
    IHttpContextAccessor accessor, 
    HybridCache cache, 
    DataContext dataContext, 
    Koworker.Mapper mapper)
{
    private static string? GetSub(ClaimsPrincipal principal) => 
        principal.FindFirstValue(ClaimTypes.NameIdentifier) ??
        principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
    
    private readonly Lazy<Func<CancellationToken, ValueTask<State?>>> _stateTask = new(() => async ct =>
    {
        var principal = accessor.HttpContext?.User;
        if (principal is null ||
            GetSub(principal) is not { } sub || 
            Guid.TryParse(sub, out var kcId) is false)
        {
            return null;
        }

        var roles = principal.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
        return await cache.GetOrCreateAsync(Koworker.GetCacheKey(kcId), factory: async innerCt =>
            {
                var user = await dataContext.Koworkers
                    .Where(x => x.KeycloakId == kcId)
                    .Project(mapper.ProjectToModels)
                    .FirstOrDefaultAsync(innerCt);

                // ReSharper disable once InvertIf
                if (user is null)
                {
                    var newUser = new Koworker
                    {
                        KeycloakId = kcId,
                        AvatarUrl = null
                    };
                    dataContext.Koworkers.Add(newUser);
                    await dataContext.SaveChangesAsync(innerCt);
                    user = mapper.ToModel(newUser);
                }

                return new State(user, mapper.Encoder.DecodeRequiredTextId(user.Id), roles);
            },
            options: new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),
                LocalCacheExpiration = TimeSpan.FromMinutes(5)
            },
            cancellationToken: ct);
    }, LazyThreadSafetyMode.ExecutionAndPublication);
    
    public async ValueTask<State> GetRequiredStateAsync(CancellationToken ct) =>
        await GetCurrentStateAsync(ct) ?? throw new InvalidOperationException("Cannot retrieve required state");

    public ValueTask<State?> GetCurrentStateAsync(CancellationToken ct) => _stateTask.Value(ct);

    public async ValueTask<long?> GetCurrentUserId(CancellationToken ct) => await GetCurrentStateAsync(ct) is { } state
        ? state.UserId
        : null;

    public async ValueTask<long> GetRequiredUserId(CancellationToken ct) =>
        (await GetRequiredStateAsync(ct)).UserId;

    public readonly record struct State(
        Koworker.Model User,
        long UserId,
        IReadOnlyCollection<string> Roles);
}