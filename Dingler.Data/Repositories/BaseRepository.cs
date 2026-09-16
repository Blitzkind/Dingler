using Microsoft.EntityFrameworkCore;

namespace Dingler.Data.Repositories;

public abstract class BaseRepository<T> where T : DbContext
{
	protected readonly IDbContextFactory<T> _factory;
	private readonly SqliteWriterQueue<T> _writerQueue;

	protected BaseRepository(IDbContextFactory<T> factory, SqliteWriterQueue<T> writerQueue)
	{
		_factory = factory;
		_writerQueue = writerQueue;
	}

	protected Task EnqueueWriteAsync(Func<T, Task> work) => _writerQueue.EnqueueWriteAsync<object?>(async db =>
	{
		await work(db);
		return null;
	});

	protected Task<TOutput> EnqueueWriteAsync<TOutput>(Func<T, Task<TOutput>> work) =>
		_writerQueue.EnqueueWriteAsync(work);
}