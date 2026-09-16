using Microsoft.EntityFrameworkCore;

namespace Dingler.Data.Sqlite;

public abstract class SqliteWriteOp
{
	public abstract Task ExecuteAsync(DbContext context);
	public abstract void Complete();
	public abstract void Fail(Exception ex);
}

public sealed class SqliteWriteOp<TContext, TOutput> : SqliteWriteOp where TContext : DbContext
{
	private readonly Func<TContext, Task<TOutput>> _work;
	private readonly TaskCompletionSource<TOutput> _tcs;
	private TOutput _result;
	
	public SqliteWriteOp(Func<TContext, Task<TOutput>> work)
	{
		_work = work;
		_tcs = new TaskCompletionSource<TOutput>(TaskCreationOptions.RunContinuationsAsynchronously);
		_result = default!;
	}

	public override async Task ExecuteAsync(DbContext context)
	{
		if (context is not TContext typedContext)
			throw new InvalidOperationException($"Expected type {typeof(TContext)} got {context.GetType()}");

		_result = await _work(typedContext);
	}

	public override void Complete() => _tcs.SetResult(_result);

	public override void Fail(Exception ex) => _tcs.SetException(ex);

	public Task<TOutput> Task => _tcs.Task;
}