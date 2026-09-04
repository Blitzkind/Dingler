extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Services;
using HexGame::Game.Client.Network.Profile;
using HexGame::Game.Shared.Network.Profile;

namespace Dingler.Game.Handlers.Request.Deck;

[Authenticated]
public sealed class RemoveDeckAsyncRequestHandler : IAsyncRequestHandler<RemoveDeckRequestArgs, RemoveDeckResponse>
{
	private readonly DeckService _deckService;

	public RemoveDeckAsyncRequestHandler(DeckService deckService)
	{
		_deckService = deckService;
	}

	public async Task<RemoveDeckResponse> HandleRequestAsync(SessionContext context, RemoveDeckRequestArgs request,
		CancellationToken token)

	{
		try
		{
			return await _deckService.RemoveDeckAsync(context, request);
		}
		catch (Exception)
		{
			return new RemoveDeckResponse()
			{
				DeckID = request.DeckID,
				Error = ERemoveDeckError.InternalServerError,
				ErrorMessage = "Could not remove deck.",
				succeded = false
			};
		}
	}
}