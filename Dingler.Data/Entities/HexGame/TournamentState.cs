namespace Dingler.Data.Entities.HexGame;

public class TournamentState
{
	public int Id { get; set; }
	public string Description { get; set; } = null!;
	public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}