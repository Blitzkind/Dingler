using Dingler.Server;
using Dingler.Game.Protocol.Chat;

namespace Dingler.Game.Tournaments;

public interface IJoinable
{
	Task<bool> TryJoinAsync(SessionContext context, TournamentJoinChatRequest request, CancellationToken token = default);
}