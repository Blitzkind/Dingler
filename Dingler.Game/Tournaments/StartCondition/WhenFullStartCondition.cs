using Dingler.Game.States;

namespace Dingler.Game.Tournaments.StartCondition;

public class WhenFullStartCondition : IStartCondition
{
	private readonly TaskCompletionSource _fullTask = new(TaskCreationOptions.RunContinuationsAsynchronously);
	
	public Task WaitForStartAsync(Tournament tournament, CancellationToken token)
	{
		if (tournament.IsFull)
			return Task.CompletedTask;

		tournament.PlayerRegistered += OnPlayerRegistered;
		return _fullTask.Task;
	}

	public void OnPlayerRegistered(Tournament tournament, TournamentRoomState state, string playerUsername)
	{
		if (tournament.IsFull)
		{
			tournament.PlayerRegistered -= OnPlayerRegistered;
			_fullTask.TrySetResult();
		}
	}
}