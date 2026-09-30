using Microsoft.EntityFrameworkCore;

namespace Dingler.Data;

internal abstract class SqliteWriteOp
{
	internal abstract Task ExecuteAsync(DbContext context);
	internal abstract void Complete();
	internal abstract void Fail(Exception ex);
}

internal sealed class SqliteWriteOp<TContext, TOutput> : SqliteWriteOp where TContext : DbContext
{
	private readonly Func<TContext, Task<TOutput>> _work;
	private readonly TaskCompletionSource<TOutput> _tcs;
	private TOutput _result;
	
	internal SqliteWriteOp(Func<TContext, Task<TOutput>> work)
	{
		_work = work;
		_tcs = new TaskCompletionSource<TOutput>(TaskCreationOptions.RunContinuationsAsynchronously);
		_result = default!;
	}

	internal override async Task ExecuteAsync(DbContext context)
	{
		if (context is not TContext typedContext)
			throw new InvalidOperationException($"Expected type {typeof(TContext)} got {context.GetType()}");

		_result = await _work(typedContext);
	}

	internal override void Complete() => _tcs.SetResult(_result);

	internal override void Fail(Exception ex) => _tcs.SetException(ex);

	internal Task<TOutput> Task => _tcs.Task;
}