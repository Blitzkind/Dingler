extern alias HexGame;
using HexGame::Game.Shared.Domain;
using HexGame::Game.Shared.Mechanics;

namespace Dingler.Game.Tournaments.Registration.Rules;

public class DeckRarityRule : IRegistrationRule
{
	private readonly ERarity _rarity;

	public DeckRarityRule(ERarity rarity)
	{
		_rarity = rarity;
	}


	public RegistrationResult Validate(string username, deck_bits deck, Tournament tournament)
	{
		foreach (var card in deck.CardsInDeck)
		{
			if (card.Template.CardRarity != _rarity)
				return RegistrationResult.Fail($"Card {card.Template.Name} is not {_rarity}");
		}
		
		foreach (var card in deck.CardsInSideboard)
		{
			if (card.Template.CardRarity != _rarity)
				return RegistrationResult.Fail($"Card {card.Template.Name} is not {_rarity}");
		}

		return RegistrationResult.Success();
	}
}