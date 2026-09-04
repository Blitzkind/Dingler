using System.Threading.Channels;

namespace Dingler.Server.Systems;

public sealed class Actor<TState>
{
	private readonly Channel<Func<TState, CancellationToken, Task>> _workerChannel =
		Channel.CreateUnbounded<Func<TState, CancellationToken, Task>>();
	
	public bool IsRunning { get; private set; } = false;
	public TState State { get; }
	public Actor(TState state)
	{
		State = state;
	}

	public Task<TResult> ScheduleWork<TResult>(Func<TState, CancellationToken, Task<TResult>> work)
	{
		var tcs = new TaskCompletionSource<TResult>();
		_workerChannel.Writer.TryWrite(async (state, processToken) =>
		{
			try
			{
				var result = await work(state, processToken);
				tcs.SetResult(result);
			}
			catch (OperationCanceledException e)
			{
				tcs.SetCanceled(e.CancellationToken);
			}
			catch (Exception e)
			{
				tcs.SetException(e);
			}
		});
		
		return tcs.Task;
	}
	
	public Task<TResult> ScheduleWork<TResult>(Func<TState, TResult> work)
	{
		var tcs = new TaskCompletionSource<TResult>();
		_workerChannel.Writer.TryWrite(async (state, processToken) =>
		{
			try
			{
				var result = await Task.Run(() => work(state), processToken);
				tcs.SetResult(result);
			}
			catch (OperationCanceledException e)
			{
				tcs.SetCanceled(e.CancellationToken);
			}

			catch (Exception e)
			{
				tcs.SetException(e);
			}
		});
		
		return tcs.Task;
	}
	
	public Task ScheduleWork(Func<TState, CancellationToken, Task> work)
	{
		var tcs = new TaskCompletionSource();
		
		_workerChannel.Writer.TryWrite(async (state, processToken) =>
		{
			try
			{
				await work(state, processToken)
					.ConfigureAwait(false);
				
				tcs.SetResult();
			}
			catch (OperationCanceledException e)
			{
				tcs.SetCanceled(e.CancellationToken);
			}

			catch (Exception e)
			{
				tcs.SetException(e);
			}
		});
		
		return tcs.Task;
	}
	
	public Task ScheduleWork(Action<TState> work)
	{
		var tcs = new TaskCompletionSource();
		
		_workerChannel.Writer.TryWrite(async (state, processToken) =>
		{
			try
			{
				await Task.Run(() => work(state), processToken);
				
				tcs.SetResult();
			}
			catch (OperationCanceledException e)
			{
				tcs.SetCanceled(e.CancellationToken);
			}

			catch (Exception e)
			{
				tcs.SetException(e);
			}
		});
		
		return tcs.Task;
	}
	
	public async Task RunAsync(CancellationToken token)
	{
		if (IsRunning)
			return;
		try
		{
			IsRunning = true;
			await foreach (var work in _workerChannel.Reader.ReadAllAsync(token))
			{
				using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
				cts.CancelAfter(TimeSpan.FromSeconds(30));
				await work(State, cts.Token);
			}
		}
		catch (OperationCanceledException)
		{

		}
		finally
		{
			IsRunning = false;
		}
	}

	public void Finish()
	{
		_workerChannel.Writer.Complete();
	}
}