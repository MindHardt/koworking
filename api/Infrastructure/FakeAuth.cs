using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Koworking.Api.Infrastructure;

public static class FakeAuth
{
    public const string Scheme = nameof(FakeAuth);
    public const string UserIdHeader = "x-user-id";
    
    public static IServiceCollection AddFakeAuth(this IServiceCollection services)
    {
        services.AddAuthentication(Scheme).AddScheme<Options, Handler>(Scheme, configureOptions: null);
        return services;
    }

    private class Options : AuthenticationSchemeOptions;

    private class Handler(IWebHostEnvironment host, IOptionsMonitor<Options> options, ILoggerFactory logger, UrlEncoder encoder) 
        : AuthenticationHandler<Options>(options, logger, encoder)
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (host.IsProduction())
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            
            var headers = Context.Request.Headers;
            if (headers.TryGetValue(UserIdHeader, out var sub) is false ||
                Guid.TryParse(sub, out var kcId) is false)
            {
                Logger.LogInformation("No user id header found");
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var principal = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, kcId.ToString())  
            ], authenticationType: FakeAuth.Scheme));
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, FakeAuth.Scheme)));
        }
    }
}