extern alias HexGame;
using HexGame::Game.Shared.Domain;
using HexGame::Game.Shared.Mechanics;
using HexGame::Game.Shared.Tournaments;

namespace Dingler.Game.Tournaments.Registration.Rules;

public class DeckFormatRule : IRegistrationRule
{
	private readonly ETournamentFormats _format;

	public DeckFormatRule(ETournamentFormats format)
	{
		_format = format;
	}
	
	public RegistrationResult Validate(string username, deck_bits deck, Tournament tournament)
	{
		if (_format.HasFlag(ETournamentFormats.Immortal))
		{
			foreach (var card in deck.CardsInDeck)
			{
				if (!Format.IsCardValidForFormat(ESetFormat.Immortal, card.TemplateID) && !card.Template.IsBasicResource())
					return RegistrationResult.Fail($"Card {card.Template.Name} is not legal in Immortal");
			}
			
			foreach (var card in deck.CardsInSideboard)
			{
				if (!Format.IsCardValidForFormat(ESetFormat.Immortal, card.TemplateID) && !card.Template.IsBasicResource())
					return RegistrationResult.Fail($"Card {card.Template.Name} is not legal in Immortal");
			}
			return RegistrationResult.Success();
		}

		if (_format == ETournamentFormats.Constructed)
		{
			foreach (var card in deck.CardsInDeck)
			{
				if (!Format.IsCardValidForFormat(ESetFormat.Standard, card.TemplateID))
					return RegistrationResult.Fail($"Card {card.Template.Name} is not legal in Standard");
			}
			
			foreach (var card in deck.CardsInSideboard)
			{
				if (!Format.IsCardValidForFormat(ESetFormat.Standard, card.TemplateID))
					return RegistrationResult.Fail($"Card {card.Template.Name} is not legal in Standard");
			}

			return RegistrationResult.Success();
		}

		return RegistrationResult.Fail($"Invalid format {_format}");
	}
}