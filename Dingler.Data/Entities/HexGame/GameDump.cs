namespace Dingler.Data.Entities.HexGame;

public class GameDump
{
	public ulong Id { get; set; }
	public string Json { get; set; } = null!;
	public ulong GameId { get; set; }
	public Game Game { get; set; } = null!;
}