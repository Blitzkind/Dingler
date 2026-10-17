namespace Dingler.Data.Entities.HexGame;

public class GameLog
{
	public int Id { get; set; }
	public ulong GameId { get; set; }
	public Game Game { get; set; } = null!;
	public string Description { get; set; } = null!;
}