extern alias HexGame;
using Dingler.Game.Tournaments.Registration;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Domain;

namespace Dingler.Game.Tournaments;

public interface IRegisterable
{
	Task<RegistrationResult> RegisterAsync(string username, deck_bits deck, UID playerUid);
}