using Dingler.Auth.Models;

namespace Dingler.Auth.AuthenticationService;

public class DefaultAuthenticationService : IAuthenticationService
{
	public Task<Dictionary<string, string>> LoginAsync(LoginRequest request)
	{
		return Task.FromResult(new Dictionary<string, string>()
		{
			{"result", "success"},
		});
	}

	public Task<bool> RegisterAsync(string email, string password)
	{
		return Task.FromResult(true);
	}
}