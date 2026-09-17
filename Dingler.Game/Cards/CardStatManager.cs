extern alias HexGame;
using HexGame::Game.Shared.Mechanics;
using HexGame::Game.Shared;

namespace Dingler.Game.Cards;

public sealed class CardStatManager
{
	private readonly Dictionary<Player, Dictionary<Card, CardSnapshot>> _cardSnapshots = new();
	private readonly Dictionary<Card, CardSnapshot> _trueSnapshots = new();

	public void TrackChangesInCards(IEnumerable<Card> cards)
	{
		foreach (var card in cards)
		{
			var snapshot = CardSnapshotFactory.Create(card);
			_trueSnapshots[card] = snapshot;
		}
	}
	
	public List<Card> FilterCardsWithUpdates(Player player, IEnumerable<Card> cards)
	{
		if (!_cardSnapshots.TryGetValue(player, out var playerSnapshots))
		{
			playerSnapshots = new Dictionary<Card, CardSnapshot>();
			_cardSnapshots[player] = playerSnapshots;
		}

		var list = new List<Card>();
		foreach (var card in cards)
		{
			if (!_trueSnapshots.TryGetValue(card, out var snapshot))
				continue;
			
			if (playerSnapshots.TryGetValue(card, out var oldSnapshot) && oldSnapshot.Hash == snapshot.Hash) 
				continue;
			
			list.Add(card);
			playerSnapshots[card] = snapshot;
		}
		
		return list;
	}
}