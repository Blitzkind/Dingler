using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;

namespace Dingler.Data.Sqlite;

public class SqliteQueue<TContext> where TContext : DbContext
{
	public Channel<SqliteWriteOp> Writes { get; } = Channel.CreateUnbounded<SqliteWriteOp>();
}