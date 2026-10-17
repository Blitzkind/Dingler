namespace Dingler.Data.Entities.HexGame;

public class DraftSet
{
    public int Id { get; set; }
    public ulong TournamentId { get; set; }
    public Tournament Tournament { get; set; }
    public int CardSetId { get; set; }
    public CardSet CardSet { get; set; }
    public int Order { get; set; }
}