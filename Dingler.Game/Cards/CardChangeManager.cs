extern alias HexGame;
using Card = HexGame::Game.Shared.Mechanics.Card;
using SessionCardCache = HexGame::Game.Shared.Mechanics.SessionCardCache;
using SessionCardId = HexGame::Game.Shared.SessionCardId;

namespace Dingler.Game.Cards;

public sealed class CardChangeManager
{
	private readonly SessionCardCache _cardCache;
	private readonly Dictionary<SessionCardId, int> _cardHashes = new();
	
	public CardChangeManager(SessionCardCache cardCache)
	{
		_cardCache = cardCache;
	}

	public int CalcHash(SessionCardId cardId)
	{
		var card = _cardCache.GetCard(cardId);

		return CalcHash(card);
	}

	public int CalcHash(Card card)
	{
		var hash = CardSnapshotFactory.HashCard(card);
		_cardHashes[card.m_SessionCardId] = hash;
		return hash;
	}

	public Dictionary<SessionCardId, int> CalcHashes(IEnumerable<SessionCardId> cardIds)
	{
		var dictionary = new Dictionary<SessionCardId, int>();
		
		foreach (var cardId in cardIds)
		{
			var hash = CalcHash(cardId);
			dictionary[cardId] = hash;
		}

		return dictionary;
	}

	public Dictionary<SessionCardId, int> CalcHashes(IEnumerable<Card> cards)
	{
		var dictionary = new Dictionary<SessionCardId, int>();
		foreach (var card in cards)
		{
			var hash = CalcHash(card);
			dictionary[card.m_SessionCardId] = hash;
		}

		return dictionary;
	}

	public List<Card> GetChangedCards(IEnumerable<SessionCardId> cardIds)
	{
		var cardList = new List<Card>();

		foreach (var cardId in cardIds)
		{
			if (!_cardHashes.TryGetValue(cardId, out var oldHash))
			{
				oldHash = 0;
				_cardHashes[cardId] = oldHash;
			}
			
			var newHash = CalcHash(cardId);
			
			if (newHash == oldHash)
				continue;

			var card = _cardCache.GetCard(cardId);
			
			cardList.Add(card);
		}

		return cardList;
	}
	
	public List<Card> GetChangedCards(IEnumerable<Card> cards)
	{
		var cardList = new List<Card>();

		foreach (var card in cards)
		{
			if (!_cardHashes.TryGetValue(card.m_SessionCardId, out var oldHash))
			{
				oldHash = 0;
				_cardHashes[card.m_SessionCardId] = oldHash;
			}
			
			var newHash = CalcHash(card);
			
			if (newHash == oldHash)
				continue;
			
			cardList.Add(card);
		}

		return cardList;
	}
}