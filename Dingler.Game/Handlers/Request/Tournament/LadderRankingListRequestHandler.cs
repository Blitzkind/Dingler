extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using static HexGame::Game.Shared.Tournaments.Messages.Tournament;

namespace Dingler.Game.Handlers.Request.Tournament;

[Authenticated]
public sealed class LadderRankingListRequestHandler : IRequestHandler<LadderRankingList.Request, LadderRankingList.Response>
{
	public LadderRankingList.Response HandleRequest(SessionContext context, LadderRankingList.Request request)

	{
		return new LadderRankingList.Response()
		{
			ConstructedLadderRanking =
			[
				"Thank",
				"You",
				"For",
				"Your",
				"Understanding",
				":)"
			],
			LimitedLadderRanking =
			[
				"I",
				"Don't",
				"Know",
				"If",
				"This",
				"Will",
				"Be",
				"Implemented"
			]
		};
	}
}