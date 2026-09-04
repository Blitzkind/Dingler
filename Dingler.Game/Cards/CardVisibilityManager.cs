extern alias HexGame;
using Card = HexGame::Game.Shared.Mechanics.Card;
using Player = HexGame::Game.Shared.Player;

namespace Dingler.Game.Cards;

public sealed class CardVisibilityManager
{
	private readonly Dictionary<Player, HashSet<Card>> _visibleCardsForPlayer = new();
	private readonly Dictionary<Card, HashSet<Player>> _playersThatCanSeeCard = new();
	
	public bool TrySetCardAsVisibleForPlayer(Player player, Card card)
	{
		if (!_visibleCardsForPlayer.TryGetValue(player, out var visibleCards))
		{
			visibleCards = new HashSet<Card>();
			_visibleCardsForPlayer[player] = visibleCards;
		}

		if (!visibleCards.Add(card))
			return false;

		if (!_playersThatCanSeeCard.TryGetValue(card, out var players))
		{
			players = new HashSet<Player>();
			_playersThatCanSeeCard[card] = players;
		}

		players.Add(player);
		return true;
	}

	public bool TrySetCardAsInvisibleForPlayer(Player player, Card card)
	{
		if (!_visibleCardsForPlayer.TryGetValue(player, out var visibleCards))
		{
			visibleCards = new HashSet<Card>();
			_visibleCardsForPlayer[player] = visibleCards;
		}

		if (!visibleCards.Remove(card))
			return false;
		
		if (!_playersThatCanSeeCard.TryGetValue(card, out var players))
		{
			players = new HashSet<Player>();
			_playersThatCanSeeCard[card] = players;
		}

		players.Remove(player);
		return false;
	}

	public List<Card> GetListOfCardsPlayerCanSee(Player player)
	{
		if (!_visibleCardsForPlayer.TryGetValue(player, out var cards))
		{
			cards = new HashSet<Card>();
		}

		return cards.ToList();
	}

	public List<Player> GetPlayersThatCanSeeCard(Card card)
	{
		if (!_playersThatCanSeeCard.TryGetValue(card, out var players))
		{
			players = new HashSet<Player>();
		}

		return players.ToList();
	}
}