namespace Dingler.Data.Entities.GameData;

public class Deck
{
    public int Id { get; set; }

    public string DeckName { get; set; } = null!;

    public Guid DeckGuid { get; set; }

    public ulong PlayerProfileId { get; set; }

    public Guid ChampionGuid { get; set; }

    public string? DeckBitsJson { get; set; }

    public virtual PlayerProfile PlayerProfile { get; set; } = null!;
}
