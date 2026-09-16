using Dingler.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Dingler.Data.Repositories;

public abstract class BaseRepository<T> where T : DbContext
{
	protected readonly IDbContextFactory<T> _factory;
	private readonly SqliteWriter<T> _writer;

	protected BaseRepository(IDbContextFactory<T> factory, SqliteWriter<T> writer)
	{
		_factory = factory;
		_writer = writer;
	}

	protected Task EnqueueWriteAsync(Func<T, Task> work) => _writer.EnqueueWriteAsync<object?>(async db =>
	{
		await work(db);
		return null;
	});

	protected Task<TOutput> EnqueueWriteAsync<TOutput>(Func<T, Task<TOutput>> work) =>
		_writer.EnqueueWriteAsync(work);
}