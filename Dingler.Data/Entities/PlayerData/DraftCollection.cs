namespace Dingler.Data.Entities.PlayerData;

public class DraftCollection
{
    public int Id { get; set; }

    public ulong TournamentId { get; set; }

    public ulong PlayerProfileId { get; set; }

    public string? CardsJson { get; set; }

    public string? DeckJson { get; set; }
}
