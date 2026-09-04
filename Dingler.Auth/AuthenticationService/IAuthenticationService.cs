using Dingler.Auth.Models;

namespace Dingler.Auth.AuthenticationService;

public interface IAuthenticationService
{
	Task<Dictionary<string, string>> LoginAsync(LoginRequest request);
	Task<bool> RegisterAsync(string email, string password);
}