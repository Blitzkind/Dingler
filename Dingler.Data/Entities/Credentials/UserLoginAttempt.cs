namespace Dingler.Data.Entities.Credentials;

public partial class UserLoginAttempt
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? FailedLoginCount { get; set; }

    public int? LastFailedLogin { get; set; }

    public int? LastSuccessfulLogin { get; set; }

    public virtual UserCredential? User { get; set; }
}
