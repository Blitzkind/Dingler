using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;

namespace Dingler.Data.Sqlite;

public class SqliteWriter<TContext> where TContext : DbContext
{
	private readonly Channel<SqliteWriteOp> _channel;

	public SqliteWriter(SqliteQueue<TContext> sqliteQueue, IDbContextFactory<TContext> factory)
	{
		_channel = sqliteQueue.Writes;
	}

	public Task<TOutput> EnqueueWriteAsync<TOutput>(Func<TContext, Task<TOutput>> work)
	{
		var op = new SqliteWriteOp<TContext, TOutput>(work);
		if (!_channel.Writer.TryWrite(op))
			throw new InvalidOperationException("Writer queue is shut down.");

		return op.Task;
	}
}