using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Dingler.Data.Sqlite;

public class SqliteReader<TContext> : BackgroundService where TContext : DbContext
{
	private readonly Channel<SqliteWriteOp> _channel;
	private readonly IDbContextFactory<TContext> _factory;
	
	public SqliteReader(SqliteQueue<TContext> sqliteQueue, IDbContextFactory<TContext> factory)
	{
		_channel = sqliteQueue.Writes;
		_factory = factory;
	}
	
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		try
		{
			await ProcessAsync(stoppingToken).ConfigureAwait(false);
		}
		catch (OperationCanceledException)
		{
			// Shutdown
		}
	}

	private async Task ProcessAsync(CancellationToken token)
	{
		var batch = new List<SqliteWriteOp>(500);
		while (await _channel.Reader.WaitToReadAsync(token)) 
		{ 
			batch.Clear(); 
			while (batch.Count < 500 && _channel.Reader.TryRead(out var op)) 
				batch.Add(op); 
			
			await RunBatchAsync(batch);
		}
	}

	private async Task RunBatchAsync(List<SqliteWriteOp> batch)
	{
		var succeeded = new List<SqliteWriteOp>();
		var failed = new List<(SqliteWriteOp op, Exception ex)>();

		await using var db = await _factory.CreateDbContextAsync();
		await using var transaction = await db.Database.BeginTransactionAsync();

		foreach (var op in batch)
		{
			await transaction.CreateSavepointAsync("op");
			try
			{
				await op.ExecuteAsync(db).ConfigureAwait(false);
				await db.SaveChangesAsync().ConfigureAwait(false);
				await transaction.ReleaseSavepointAsync("op").ConfigureAwait(false);
				succeeded.Add(op);
			}
			catch (Exception ex)
			{
				await transaction.RollbackToSavepointAsync("op");
				db.ChangeTracker.Clear();
				failed.Add((op, ex));
			}
		}
		
		foreach (var (op, ex) in failed) op.Fail(ex);
		if (succeeded.Count == 0)
			return;
		
		try
		{
			await transaction.CommitAsync();
		}
		catch (Exception ex)
		{
			foreach (var op in succeeded) op.Fail(ex);
			return;
		}

		foreach (var op in succeeded) op.Complete();
	}
}