namespace Dingler.Data.Entities.HexGame;

public class Game
{
	public ulong Id { get; set; }
	public ulong TournamentId { get; set; }
	public int GameStateId { get; set; }
	public GameState GameState { get; set; } = null!;
	public Tournament Tournament { get; set; } = null!;
	public ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();
}