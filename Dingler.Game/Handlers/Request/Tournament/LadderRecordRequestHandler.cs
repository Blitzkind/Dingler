extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using static HexGame::Game.Shared.Tournaments.Messages.Tournament;

namespace Dingler.Game.Handlers.Request.Tournament;

extern alias HexGame;

[Authenticated]
public sealed class LadderRecordRequestHandler : IRequestHandler<LadderRecord.Request, LadderRecord.Response>
{
	public LadderRecord.Response HandleRequest(SessionContext context, LadderRecord.Request request)
	{
		return new LadderRecord.Response();
	}
}