namespace Dingler.Data.Entities.Credentials;

public partial class UserCredential
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual BannedUser? BannedUser { get; set; }

    public virtual UserLoginAttempt? UserLoginAttempt { get; set; }
}
