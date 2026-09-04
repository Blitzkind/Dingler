using Dingler.Auth.AuthenticationService;
using Dingler.Auth.Models;

namespace Dingler.Auth.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app
            .MapGet("/auth/hexlogin", async (string user, string pass, string region, string lang, string? totp, IAuthenticationService authService) =>
            {
                var loginRequest = new LoginRequest(user, pass, region, lang, totp ?? "");

                var result = await authService.LoginAsync(loginRequest).ConfigureAwait(false);
                
                return result;
            });
            return app;
        }
    }
}
