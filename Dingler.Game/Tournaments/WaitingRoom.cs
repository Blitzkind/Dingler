extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Systems;
using Dingler.Game.Games;
using Dingler.Game.Protocol.Chat;
using Dingler.Game.States;
using Dingler.Game.Tournaments.Registration;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Domain;

namespace Dingler.Game.Tournaments;

public class WaitingRoom : IRegisterable, IJoinable
{
	private readonly Actor<WaitingRoomState> _actor;
	private readonly TournamentManager _tournamentManager;
	private readonly TournamentSettings _tournamentSettings;
	private readonly GameSettings _gameSettings;

	public WaitingRoom(TournamentManager tournamentManager, TournamentSettings tournamentSettings,
		GameSettings gameSettings)
	{
		_tournamentManager = tournamentManager;
		_tournamentSettings = tournamentSettings;
		_gameSettings = gameSettings;
		var tournament = tournamentManager.CreateTournament(tournamentSettings, gameSettings);
		_actor = new Actor<WaitingRoomState>(
			new WaitingRoomState(tournament));
	}

	public Task RunAsync(CancellationToken token)
	{
		return _actor.RunAsync(token);
	}

	// Honestly I'm not sure if this is necessary. Might just be able to give CurrentTournament to this class and
	// skip the actor altogether. It works for now so I'm whatever about it.
	public Task<RegistrationResult> RegisterAsync(string username, deck_bits deck, UID playerUid)
	{
		return _actor.ScheduleWork(async (state, _) =>
		{
			var result = await state.CurrentTournament.RegisterAsync(username, deck, playerUid);

			if (state.CurrentTournament.IsFull)
				state.CurrentTournament = _tournamentManager.CreateTournament(_tournamentSettings, _gameSettings);

			return result;
		});
	}

	// Not sure this ever gets hit
	public Task<bool> TryJoinAsync(SessionContext context, TournamentJoinChatRequest request, CancellationToken token = default)
	{
		return _actor.ScheduleWork( (state, workToken) =>
			state.CurrentTournament.TryJoinAsync(context, request, workToken));
	}
}