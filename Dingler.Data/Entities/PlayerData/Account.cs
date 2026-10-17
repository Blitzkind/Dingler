namespace Dingler.Data.Entities.PlayerData;

public class Account
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public virtual PlayerProfile? PlayerProfile { get; set; }
}
