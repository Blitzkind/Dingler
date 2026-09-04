extern alias HexGame;
using HexGame::Game.Shared.Domain;
using HexGame::Game.Shared.Tournaments;

namespace Dingler.Game.Tournaments.Registration.Rules;

public class DeckSizeRule : IRegistrationRule
{
	private const int CONSTRUCTED_DECK_MINIMUM = 60;
	private const int DECK_MAXIMUM = 300;
	private const int CONSTRUCTED_RESERVES_MAXIMUM = 15;

	private const int LIMITED_DECK_MINIMUM = 40;

	private readonly ETournamentFormats _format;

	public DeckSizeRule(ETournamentFormats format)
	{
		_format = format;
	}
	
	public RegistrationResult Validate(string username, deck_bits deck, Tournament tournament)
	{
		if (_format == ETournamentFormats.Constructed || _format.HasFlag(ETournamentFormats.Immortal))
		{
			return deck.CardsInDeck.Count < CONSTRUCTED_DECK_MINIMUM || deck.CardsInDeck.Count > DECK_MAXIMUM ||
			        deck.CardsInSideboard.Count > CONSTRUCTED_RESERVES_MAXIMUM
				? RegistrationResult.Fail("Deck was an invalid size")
				: RegistrationResult.Success();
		}

		// assume limited
		return deck.CardsInDeck.Count < LIMITED_DECK_MINIMUM
			? RegistrationResult.Fail("Deck was an invalid size")
			: RegistrationResult.Success();
	}
}