namespace Dingler.Data.Entities.GameData;

public class StartCondition
{
    public int Id { get; set; }
    public string Description { get; set; }
    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}