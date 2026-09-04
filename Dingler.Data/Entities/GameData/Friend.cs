namespace Dingler.Data.Entities.GameData;

public class Friend
{
    public int Id { get; set; }

    public ulong RequesterId { get; set; }

    public ulong RequestedId { get; set; }

    public int FriendStatusId { get; set; }
    public virtual FriendStatus Status { get; set; } = null!;

    public virtual PlayerProfile Requested { get; set; } = null!;

    public virtual PlayerProfile Requester { get; set; } = null!;
}
