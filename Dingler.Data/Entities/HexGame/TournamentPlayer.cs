namespace Dingler.Data.Entities.HexGame;

public class TournamentPlayer
{
	public int Id { get; set; }
	public ulong TournamentId { get; set; }
	public Tournament Tournament { get; set; } = null!;
	public ulong PlayerInfoId { get; set; }
	public ulong DeckId { get; set; }
}