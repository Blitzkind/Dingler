extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Services;
using HexGame::Game.Client.Network.Profile;
using HexGame::Game.Shared.Network.Profile;

namespace Dingler.Game.Handlers.Request.Deck;

[Authenticated]
public sealed class UpdateDeckAsyncRequestHandler : IAsyncRequestHandler<UpdateDeckRequestArgs, UpdateDeckResponse>
{
	private readonly DeckService _deckService;

	public UpdateDeckAsyncRequestHandler(DeckService deckService)
	{
		_deckService = deckService;
	}

	public async Task<UpdateDeckResponse> HandleRequestAsync(SessionContext context, UpdateDeckRequestArgs request,
		CancellationToken token)
	{
		try
		{
			return await _deckService.UpdateDeckAsync(context, request);
		}
		catch (Exception)
		{
			return new UpdateDeckResponse()
			{
				DeckID = request.DeckID,
				Error = EUpdateDeckError.InternalServerError,
				ErrorMessage = "Could not update deck."
			};
		}
	}
}