extern alias HexGame;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Dingler.Server;
using Dingler.Game.Games;
using Dingler.Game.Tournaments;
using HexGame::Game.Shared.Tournaments;

namespace Dingler.Game.Match;

public class MatchManager
{
	private readonly ConcurrentDictionary<ulong, TournamentMatch> _matches;
	private readonly ConcurrentDictionary<string, ulong> _matchPlayerIsIn;
	private readonly GameManager _gameManager;
	private readonly GameSettings _gameSettings;
	private readonly TournamentInfo _tournamentInfo;
	private readonly MatchCommunicator _matchCommunicator;
	private ulong _currentMatchId;

	public MatchManager(GameManager gameManager, GameSettings gameSettings, TournamentInfo tournamentInfo,
		SessionManager sessionManager)
	{
		_matches = new ConcurrentDictionary<ulong, TournamentMatch>();
		_matchPlayerIsIn = new ConcurrentDictionary<string, ulong>();
		_gameManager = gameManager;
		_currentMatchId = 0;
		_gameSettings = gameSettings;
		_tournamentInfo = tournamentInfo;
		_matchCommunicator = new MatchCommunicator(sessionManager);
	}

	public TournamentMatch CreateMatch(TournamentPairing pairing)
	{
		var match = new TournamentMatch(_currentMatchId, _gameManager, _gameSettings, pairing, _tournamentInfo);

		_matches[_currentMatchId] = match;
		_matchPlayerIsIn[pairing.Player1.Name] = _currentMatchId;
		_matchPlayerIsIn[pairing.Player2.Name] = _currentMatchId;
		_currentMatchId++;

		match.MatchEnded += OnMatchEnded;
		_matchCommunicator.RegisterEvents(match);
		return match;
	}

	public bool TryGetMatchForPlayer(string username, [MaybeNullWhen(false)] out TournamentMatch tournamentMatch)
	{
		if (!_matchPlayerIsIn.TryGetValue(username, out var matchId))
		{
			tournamentMatch = null;
			return false;
		}

		return _matches.TryGetValue(matchId, out tournamentMatch);
	}

	private void OnMatchEnded(TournamentMatch match)
	{
		match.MatchEnded -= OnMatchEnded;
		_matchCommunicator.UnregisterEvents(match);
		_matches.Remove(match.MatchId, out _);

		foreach (var player in match.GetPlayersInMatch())
		{
			_matchPlayerIsIn.Remove(player, out _);
		}
		
	}
}