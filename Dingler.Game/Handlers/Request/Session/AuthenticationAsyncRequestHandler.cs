extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Game.Extensions;
using Dingler.Game.Protocol.Messages.Requests;
using Dingler.Game.Services;
using HexGame::Game.Shared.Network.SFS;

namespace Dingler.Game.Handlers.Request.Session;

public sealed class AuthenticationAsyncRequestHandler : IAsyncRequestHandler<AuthenticationRequestArg, ClientConnection.AuthInfo>
{
	private readonly SessionService _sessionService;
	private readonly SessionManager _sessionManager;

	public AuthenticationAsyncRequestHandler(SessionService sessionService, SessionManager sessionManager)
	{
		_sessionService = sessionService;
		_sessionManager = sessionManager;
	}

	public async Task<ClientConnection.AuthInfo> HandleRequestAsync(SessionContext context,
		AuthenticationRequestArg request, CancellationToken token)

	{
		var response = await _sessionService.GetAuthenticationResponseAsync(request)
			.ConfigureAwait(false);

		if (response.Success)
		{
			context.UserName = request.UserName;
			context.SetProfileId(response.reckID.GetInstanceId());
			context.SetAccountId(response.authID.GetInstanceId());
			context.IsAuthenticated = true;

			_sessionManager.TryLinkUserToSession(context.UserName, context);
		}

		return response;
	}
}