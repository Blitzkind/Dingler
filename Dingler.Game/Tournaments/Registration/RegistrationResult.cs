namespace Dingler.Game.Tournaments.Registration;

public class RegistrationResult
{
	public bool IsSuccess { get; }
	public ulong TournamentId { get; set; }
	public string? FailureReason { get; }

	private RegistrationResult(bool success, string? reason)
	{
		IsSuccess = success;
		FailureReason = reason;
	}

	public static RegistrationResult Success() => new(true, null);
	public static RegistrationResult Fail(string reason) => new(false, reason);
}