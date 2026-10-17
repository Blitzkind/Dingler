namespace Dingler.Data.Entities.HexGame;

public class GamePlayer
{
	public ulong Id { get; set; }
	public ulong PlayerId { get; set; }
	public ulong GameId { get; set; }
	public Game Game { get; set; } = null!;
}