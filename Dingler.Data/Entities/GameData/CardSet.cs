namespace Dingler.Data.Entities.GameData;

public class CardSet
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<DraftSet> DraftSets { get; set; }
}