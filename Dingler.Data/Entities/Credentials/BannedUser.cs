namespace Dingler.Data.Entities.Credentials;

public partial class BannedUser
{
    public int Id { get; set; }

    public int UserCredentialsId { get; set; }

    public int DateOfBan { get; set; }

    public int LengthOfBan { get; set; }

    public int OffenseId { get; set; }

    public virtual BannableOffense Offense { get; set; } = null!;

    public virtual UserCredential UserCredentials { get; set; } = null!;
}
