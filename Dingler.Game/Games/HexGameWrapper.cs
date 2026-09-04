extern alias HexGame;
using System.Collections.Concurrent;
using Dingler.Server;
using Dingler.Game.Cards;
using Dingler.Game.GameObjects;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Mechanics.Transactions;
using HexGame::Game.Shared.Network.GameSession;
using Microsoft.Extensions.Logging;

namespace Dingler.Game.Games;

public class HexGameWrapper : IDisposable
{
	private readonly HexRulesEngine _gameSession;
	private readonly Dictionary<Player, Queue<(int eventId, byte[] eventData)>> _eventQueues;
	private readonly ConcurrentDictionary<Player, byte> _readyPlayers;
	private readonly Action<ulong>? _onMatchEnded;
	private readonly SessionManager _sessionManager;
	private readonly ILogger<HexGameWrapper>? _logger;
	private readonly CancellationToken _cancellationToken;
	private readonly ConcurrentDictionary<string, bool> _leaveChecks;

	public SessionStateEncounterData EncounterData => _gameSession.EncounterData;
	
	public Dictionary<string, UID> DeckIdsByPlayer
	{
		get
		{
			var dict = new Dictionary<string, UID>();

			foreach (var player in _gameSession.GetAllPlayers())
			{
				dict[player.m_ChampionCard.GetName()] = player.m_DeckID;
			}

			return dict;
		}
	}
	
	public ulong Id => _gameSession.m_SessionId.GetInstanceId();
	public HexGameWrapper(HexRulesEngine gameSession, CardVisibilityManager cardVisibilityManager,
		SessionManager sessionManager, CancellationToken cancellationToken = default, ILogger<HexGameWrapper>? logger = null,
		Action<ulong>? onMatchEnded = null)
	{
		_gameSession = gameSession;
		_sessionManager = sessionManager;
		_onMatchEnded = onMatchEnded;
		_eventQueues = new Dictionary<Player, Queue<(int eventId, byte[] eventData)>>();
		_readyPlayers = new ConcurrentDictionary<Player, byte>();
		_logger = logger;
		_gameSession.DispatchToPlayer += OnDispatchEventToPlayer;
		_gameSession.FlushReady += OnFlushReady;
		_cancellationToken = cancellationToken;
		_leaveChecks = new ConcurrentDictionary<string, bool>();
	}
	
	public bool IsGameEnded { get; private set; }

	public Task<(UID, UID)> RunGameAsync()
	{
		return RunGameAsync(UID.Invalid);
	}
	
	public async Task<(UID, UID)> RunGameAsync(UID forcedFirstPlayer)
	{
		List<UID> results = new List<UID>();
		try
		{
			_gameSession.ForcedFirstPlayer = forcedFirstPlayer;
			results =await _gameSession.RunAsync(_cancellationToken).ConfigureAwait(false);
		}
		catch (OperationCanceledException)
		{
			// ignored
		}
		catch (Exception ex)
		{
			_logger?.LogError("Match {MatchId} run loop failed: {Exception}", Id, ex);
		}
		finally
		{
			_onMatchEnded?.Invoke(Id);
		}

		if (results.Count > 1)
			return (results[0], results[1]);

		return (UID.Invalid, UID.Invalid);
	}
	
	public SessionState GetSessionState(UID playerId)
	{
		var encounterData = _gameSession.m_EncounterData;
		if (encounterData?.TournamentDecks is { } decks)
		{
			encounterData = new SessionStateEncounterData
			{
				SessionFlags = encounterData.SessionFlags,
				TournamentID = encounterData.TournamentID,
				SeriesFormat = encounterData.SeriesFormat,
				SeriesMaxGames = encounterData.SeriesMaxGames,
				MatchPreviousWinners = encounterData.MatchPreviousWinners,
				FirstPlayer = encounterData.FirstPlayer,
				TournamentDecks = decks.Where(d => d.PlayerUID == playerId).ToList()
			};
		}

		var matchPlayerCount = _gameSession.GetAllPlayers().Count;

		return new SessionState
		{
			SessionId = _gameSession.m_SessionId,
			SessionName = _gameSession.m_SessionName,
			MinimumPlayerCount = matchPlayerCount,
			MaximumPlayerCount = matchPlayerCount,
			EncounterData = encounterData,
			JoinInsteadOfReconnect = false
		};
	}
	
	public void PlayerIsReadyForEvents(UID playerId)
	{
		var player = _gameSession.GetPlayer(playerId);
		if (player is null)
			return;

		_readyPlayers.TryAdd(player, 1);
		_gameSession.ProcessWork();
	}
	
	public bool TryAddPlayer(TrackedPlayer player)
	{
		_gameSession.AddPlayer(player);
		return true;
	}
	
	public IReadOnlyList<string> GetPlayerNames()
	{
		return _gameSession.GetAllPlayers()
			.Select(p => p.m_ChampionCard?.GetName())
			.Where(name => !string.IsNullOrEmpty(name))
			.Cast<string>()
			.ToList();
	}
	
	public HexGame::Game.Shared.Network.LoadBalancer.ReadyForGameSetupResponseArgs BuildGameSetupResponse(UID playerId)
	{
		var player = _gameSession.GetPlayer(playerId);
		var opponents = player is null
			? new List<Player>()
			: _gameSession.GetOpponentsOfPlayer(player);

		var deckId = UID.Invalid;
		if (_gameSession.m_EncounterData?.TournamentDecks is { } decks)
		{
			var deckEntry = decks.FirstOrDefault(d => d.PlayerUID == playerId);
			if (deckEntry?.PlayerDeck is { } deck)
				deckId = new UID(UID.Type.Deck, deck.Id);
		}

		return new HexGame::Game.Shared.Network.LoadBalancer.ReadyForGameSetupResponseArgs
		{
			SessionState = GetSessionState(playerId),
			DeckId = deckId,
			DeckTemplateId = ResourceId.Invalid,
			OpponentsInfo = opponents
				.Select(o => new PlayerState { PlayerId = o.m_PlayerId, PlayerPosition = o.m_PlayerPosition })
				.ToList(),
			TurnOrder = _gameSession.GetAllPlayersInTurnOrder().Select(p => p.m_PlayerId).ToList(),
			seedZ = 0,
			seedW = 0
		};
	}

	public void QueueTransaction(Transaction transaction)
	{
		transaction.Initialize();
		_gameSession.SubmitTransaction(transaction);
	}
	
	private void OnFlushReady()
	{

		foreach (var player in _gameSession.GetAllPlayers())
		{
			if (!_readyPlayers.TryGetValue(player, out _))
				continue;

			if (!_eventQueues.TryGetValue(player, out var queue))
			{
				queue = new Queue<(int eventId, byte[] data)>();
				_eventQueues[player] = queue;
			}
			
			if (!_sessionManager.TryGetUserSession(player.m_ChampionCard.GetName(), out var session))
				continue;

			var networkSessionEvent = new NetworkPacketSessionEventArgs
			{
				PlayerId = player.m_PlayerId,
				SessionId = _gameSession.m_SessionId
			};
			
			const int gameStartedClass = 1;
			const int playerUpdatedClass = 65;

			var queued = new List<(int eventId, byte[] eventData)>();
			while (queue.TryDequeue(out var e))
			{
				queued.Add(e);
			}

			if (queued.Count == 0)
				continue;
			
			foreach (var e in queued.Where(e => e.eventId == gameStartedClass))
			{
				networkSessionEvent.EventIds.Add(e.eventId);
				networkSessionEvent.EventData.Add(e.eventData);
			}

			foreach (var e in queued.Where(e => e.eventId == playerUpdatedClass))
			{
				networkSessionEvent.EventIds.Add(e.eventId);
				networkSessionEvent.EventData.Add(e.eventData);
			}

			foreach (var e in queued.Where(e => e.eventId != gameStartedClass && e.eventId != playerUpdatedClass))
			{
				networkSessionEvent.EventIds.Add(e.eventId);
				networkSessionEvent.EventData.Add(e.eventData);
			}

			try
			{
				var sessionEvent = new SessionSyncEventEventArgs
				{
					RoutingPlayerId = player.m_PlayerId,
					SessionArgs = networkSessionEvent
				};

				session.TrySendMessageToClient(sessionEvent);
			}
			catch (OperationCanceledException)
			{
				return;
			}
			catch (Exception ex)
			{
				_logger?.LogError("Failed to send session event batch to player {PlayerId}: {Exception}",
					player.m_PlayerId, ex);
			}
		}
	}
	
	private void OnDispatchEventToPlayer(Player player, SessionEventArgs args)
	{
		if (!_eventQueues.TryGetValue(player, out var queue))
		{
			queue = new Queue<(int eventId, byte[] data)>();
			_eventQueues[player] = queue;
		}

		queue.Enqueue((args.Class, args.ToByteArray()));
	}

	public void Dispose()
	{
		_gameSession.DispatchToPlayer -= OnDispatchEventToPlayer;
		_gameSession.FlushReady -= OnFlushReady;
		_gameSession.Dispose();
	}
}