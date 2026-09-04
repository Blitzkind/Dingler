extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using static HexGame::Game.Shared.Tournaments.Messages.Tournament;
namespace Dingler.Game.Handlers.Request.Tournament;

extern alias HexGame;


[Authenticated]
public sealed class LadderTierRewardsReuestHandler
	: IRequestHandler<LadderTierRewardsList.Request, LadderTierRewardsList.Response>
{
	public LadderTierRewardsList.Response HandleRequest(SessionContext context,
		LadderTierRewardsList.Request request)
	{
		return new LadderTierRewardsList.Response()
		{
			tier_rewards = new()
		};
	}
}