namespace Dingler.Data.Entities.GameData;

public class PlayerProfile
{
    public ulong Id { get; set; }

    public string Username { get; set; } = null!;

    public int Elo { get; set; }
    
    public int RankId { get; set; }
    public Rank Rank { get; set; } = null!;
    
    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Deck> Decks { get; set; } = new List<Deck>();

    public virtual ICollection<Friend> FriendRequesteds { get; set; } = new List<Friend>();

    public virtual ICollection<Friend> FriendRequesters { get; set; } = new List<Friend>();
}
