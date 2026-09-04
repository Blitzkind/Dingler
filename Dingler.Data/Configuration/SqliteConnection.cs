using Microsoft.Data.Sqlite;

namespace Dingler.Data.Configuration;

public static class SqliteConnection
{
	public static string ResolveDataSource(string? connectionString)
	{
		if (string.IsNullOrEmpty(connectionString))
			return "";

		var builder = new SqliteConnectionStringBuilder(connectionString);

		if (!string.IsNullOrEmpty(builder.DataSource) && !Path.IsPathRooted(builder.DataSource))
		{
			builder.DataSource = Path.GetFullPath(builder.DataSource, AppContext.BaseDirectory);
		}

		return builder.ConnectionString;
	}

	public static void EnsureDirectoryExists(string connectionString)
	{
		if (string.IsNullOrEmpty(connectionString))
			return;

		var directory = Path.GetDirectoryName(new SqliteConnectionStringBuilder(connectionString).DataSource);

		if (!string.IsNullOrEmpty(directory))
			Directory.CreateDirectory(directory);
	}
}