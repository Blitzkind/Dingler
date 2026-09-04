namespace Dingler.Game.Tournaments.StartCondition;

public class ScheduledStartCondition : IStartCondition
{
	private readonly DateTime _starTime;

	public ScheduledStartCondition(DateTime startTime) => _starTime = startTime;
	
	public Task WaitForStartAsync(Tournament tournament, CancellationToken token)
	{
		var delay = _starTime - DateTime.UtcNow;

		return delay <= TimeSpan.Zero ? Task.CompletedTask : Task.Delay(delay, token);
	}
}