namespace Dingler.Data.Entities.GameData;

public class Rank
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<PlayerProfile> PlayerProfiles { get; set; } = new List<PlayerProfile>();
}