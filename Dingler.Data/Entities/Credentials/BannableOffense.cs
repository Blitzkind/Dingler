namespace Dingler.Data.Entities.Credentials;

public partial class BannableOffense
{
    public int Id { get; set; }

    public string Offense { get; set; } = null!;

    public virtual ICollection<BannedUser> BannedUsers { get; set; } = new List<BannedUser>();
}
