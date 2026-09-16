using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dingler.Data.Sqlite;

public static class IServiceCollectionExtensions
{
	public static IServiceCollection AddSqliteDbContext<TContext>(this IServiceCollection serviceCollection,
		string? connectionString = null)
		where TContext : DbContext
	{
		serviceCollection
			.AddDbContextFactory<TContext>(options =>
			{
				options.UseSqlite(connectionString ?? "");
			})
			.AddHostedService<SqliteReader<TContext>>()
			.AddSingleton<SqliteQueue<TContext>>()
			.AddSingleton<SqliteWriter<TContext>>();

		return serviceCollection;
	}
}