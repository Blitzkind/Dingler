namespace Dingler.Data.Entities.PlayerData;

public class FriendStatus
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public virtual ICollection<Friend> Friends { get; set; } = new List<Friend>();
}