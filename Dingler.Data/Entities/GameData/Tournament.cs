namespace Dingler.Data.Entities.GameData;

public class Tournament
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public int NeededPlayers { get; set; }
    public DateTime StartDate { get; set; }
    public int StartConditionId { get; set; }
    public StartCondition StartCondition { get; set; } = null!;
    public ICollection<DraftSet> DraftSets { get; set; }
    public int TournamentTypeId { get; set; }
    public TournamentType TournamentType { get; set; } = null!;
    public int MatchTypeId { get; set; }
    public MatchType MatchType { get; set; } = null!;
}