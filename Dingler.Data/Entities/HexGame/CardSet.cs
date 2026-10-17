using Dingler.Data.Entities.HexGame;

namespace Dingler.Data.Entities.HexGame;

public class CardSet
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<DraftSet> DraftSets { get; set; }
}