extern alias HexGame;
using HexGame::Game.Shared;

namespace Dingler.Game.Cards;

public record CardSnapshot(SessionCardId Id, int Hash);