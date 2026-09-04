extern alias HexGame;
using Card = HexGame::Game.Shared.Mechanics.Card;
using Player = HexGame::Game.Shared.Player;

namespace Dingler.Game.Cards;

public sealed class CardStatManager
{
	private readonly Dictionary<Player, Dictionary<Card, CardSnapshot>> _cardSnapshots = new();

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
			var snapshot = CardSnapshotFactory.Create(card);

			if (playerSnapshots.TryGetValue(card, out var oldSnapshot) && oldSnapshot.Hash == snapshot.Hash) 
				continue;
			
			list.Add(card);
			playerSnapshots[card] = snapshot;
		}

		return list;
	}
}