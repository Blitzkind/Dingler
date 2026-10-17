namespace Dingler.Data.Entities.GameData;

public class TournamentType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Tournament> Tournaments { get; set; }
}