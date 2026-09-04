namespace Dingler.Game.Tournaments.StartCondition;

public interface IStartCondition
{
	Task WaitForStartAsync(Tournament tournament, CancellationToken token);
}