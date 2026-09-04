extern alias HexGame;
using HexGame::Game.Shared.Domain;

namespace Dingler.Game.Tournaments.Registration.Rules;

public interface IRegistrationRule
{
	RegistrationResult Validate(string username, deck_bits deck, Tournament tournament);
}