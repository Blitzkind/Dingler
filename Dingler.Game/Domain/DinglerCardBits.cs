extern alias HexGame;
using ECardStatType = HexGame::Game.Shared.Domain.ECardStatType;
using EGemTypesNew = HexGame::Game.Shared.Mechanics.EGemTypesNew;
using ResourceId = HexGame::Game.Shared.ResourceId;

namespace Dingler.Game.Domain;

public sealed class DinglerCardBits()
{
	public ulong Id { get; set; }
	public ResourceId TemplateId { get; set; }
	public Dictionary<ECardStatType, int>? CardStats { get; set; }
	public bool IsFoil { get; set; }
	public bool IsExtended { get; set; }
	public EGemTypesNew SocketedGems { get; set; }
	public bool IsNotTradeable { get; set; }
	public string? EscrowStatus { get; set; }
}