namespace Dingler.Data.Entities.HexGame;

public class MatchType
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}