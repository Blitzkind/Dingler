extern alias HexGame;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Dingler.Server;
using Dingler.Game.Cards;
using Dingler.Game.GameObjects;
using Dingler.Game.Tournaments;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Tournaments;
using Microsoft.Extensions.Logging;

namespace Dingler.Game.Games;

public sealed class GameManager : IDisposable
{
	private readonly ConcurrentDictionary<ulong, HexGameWrapper> _runningMatches = new();
	private readonly ConcurrentDictionary<string, HexGameWrapper> _gamePlayerIsIn = new();
	private readonly SessionManager _sessionManager;
	private readonly ILoggerFactory? _loggerFactory;
	private readonly ILogger<GameManager>? _logger;
	private ulong _currentMatchId;
	private readonly Dictionary<ulong, CancellationTokenSource> _gameCtsCollection;

	public GameManager(SessionManager sessionManager, ILoggerFactory? loggerFactory)
	{
		_sessionManager = sessionManager;
		_logger = loggerFactory?.CreateLogger<GameManager>();
		_loggerFactory = loggerFactory;
		_gameCtsCollection = new Dictionary<ulong, CancellationTokenSource>();
	}

	public HexGameWrapper CreateGameSession(ulong tournamentId, TournamentPairing pairing,
		SessionStateEncounterData encounterData, CancellationToken tournamentToken)
	{
		var assignedId = Interlocked.Increment(ref _currentMatchId);
		var sessionUid = new UID(UID.Type.AuthoritativeSession, assignedId);

		var engine = new HexRulesEngine($"game-{assignedId}", sessionUid, _loggerFactory?.CreateLogger<HexRulesEngine>())
		{
			m_EncounterData = encounterData,
			ForcedFirstPlayer = encounterData.FirstPlayer
		};

		var gameCts = CancellationTokenSource.CreateLinkedTokenSource(tournamentToken);
		_gameCtsCollection[assignedId] = gameCts;
		
		var wrapper = new HexGameWrapper(engine, new CardVisibilityManager(), _sessionManager, gameCts.Token,
			_loggerFactory?.CreateLogger<HexGameWrapper>(), CleanupMatch);

		wrapper.TryAddPlayer(new TrackedPlayer(new PlayerState
		{
			PlayerId = pairing.Player1.PlayerUID,
			PlayerPosition = 0
		}, UID.Invalid));

		wrapper.TryAddPlayer(new TrackedPlayer(new PlayerState
		{
			PlayerId = pairing.Player2.PlayerUID,
			PlayerPosition = 1
		}, UID.Invalid));

		try
		{
			if (!engine.InitializeGame())
				throw new InvalidOperationException($"Match {assignedId} failed to initialize (deck load failed).");
		}
		catch (Exception ex)
		{
			_logger?.LogError(
				"Match {MatchId} creation failed during engine initialization: {Exception}", assignedId, ex);
			throw;
		}

		_runningMatches[assignedId] = wrapper;
		_gamePlayerIsIn[pairing.Player1.Name] = wrapper;
		_gamePlayerIsIn[pairing.Player2.Name] = wrapper;

		_logger?.LogInformation("Match {MatchId} created for tournament {TournamentId}: {Player1} vs {Player2}",
			assignedId, tournamentId, pairing.Player1.Name, pairing.Player2.Name);

		return wrapper;
	}

	public HexGameWrapper CreateGameSession(ulong tournamentId, TournamentPairing pairing,
		SessionStateEncounterData.SeriesType tournamentType, ESessionFlags tournamentFlags,
		CancellationToken tournamentToken)
	{
		var encounterData = BuildSessionStateEncounterData(tournamentId, pairing, tournamentType, tournamentFlags);
		return CreateGameSession(tournamentId, pairing, encounterData, tournamentToken);
	}

	private SessionStateEncounterData BuildSessionStateEncounterData(ulong tournamentId, TournamentPairing pairing,
		SessionStateEncounterData.SeriesType tournamentType, ESessionFlags tournamentFlags)
	{
		var player1Uid = pairing.Player1.PlayerUID;
		var player2Uid = pairing.Player2.PlayerUID;
		var firstPlayer = Random.Shared.Next(2) == 0 ? player1Uid : player2Uid;
		var encounterData = new SessionStateEncounterData()
		{
			SeriesFormat = tournamentType,
			SessionFlags = tournamentFlags,
			TournamentDecks = new List<TournamentDeckBitsWrapper>(),
			TournamentID = tournamentId,
			MatchPreviousWinners = new List<ulong>(),
			SeriesMaxGames = 3,
			FirstPlayer = firstPlayer
		};

		encounterData.TournamentDecks.Add(new TournamentDeckBitsWrapper()
		{
			PlayerName = pairing.Player1.Name,
			PlayerDeck = pairing.Decks[pairing.Player1.Name],
			PlayerUID = player1Uid
		});

		encounterData.TournamentDecks.Add(new TournamentDeckBitsWrapper()
		{
			PlayerName = pairing.Player2.Name,
			PlayerDeck = pairing.Decks[pairing.Player2.Name],
			PlayerUID = player2Uid
		});

		return encounterData;
	}

	private void CleanupMatch(ulong matchId)
	{
		if (_runningMatches.TryRemove(matchId, out var match))
		{
			foreach (var kvp in _gamePlayerIsIn.Where(kvp => kvp.Value == match).ToList())
			{
				_gamePlayerIsIn.TryRemove(kvp.Key, out _);
			}

			_logger?.LogInformation("Match {MatchId} removed from registry",
				matchId);
		}

		_gameCtsCollection.Remove(matchId, out var cts);

		if (cts is not null)
		{
			if (!cts.IsCancellationRequested)
				cts.Cancel();
			
			cts.Dispose();
		}
		
		match?.Dispose();
	}

	public bool TryGetMatch(ulong id, [MaybeNullWhen(false)] out HexGameWrapper match)
	{
		return _runningMatches.TryGetValue(id, out match);
	}

	public bool TryGetGameForPlayer(string username, [MaybeNullWhen(false)] out HexGameWrapper session)
	{
		return _gamePlayerIsIn.TryGetValue(username, out session);
	}

	public IReadOnlyList<string> GetRegisteredPlayerUsernames()
	{
		return _gamePlayerIsIn.Keys.ToList();
	}

	public void Cancel(ulong id)
	{
		if (!_gameCtsCollection.TryGetValue(id, out var cts))
			return;
		
		cts.Cancel();
		cts.Dispose();
	}

	public void Dispose()
	{
		_loggerFactory?.Dispose();

		foreach (var kvp in _gameCtsCollection)
		{
			if (!kvp.Value.IsCancellationRequested)
			{
				kvp.Value.Cancel();
			}

			kvp.Value.Dispose();
		}
		
		_gameCtsCollection.Clear();
	}
}
