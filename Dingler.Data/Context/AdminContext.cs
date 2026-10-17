using Dingler.Data.Entities.Admin;
using Microsoft.EntityFrameworkCore;

namespace Dingler.Data.Context;

public partial class AdminContext : DbContext
{
	public AdminContext()
	{
	}
	
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlite("Data Source=data/admin.db");
		}
	}
	
	public virtual DbSet<ApiKey> ApiKeys { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ApiKey>(entity =>
		{
			entity.HasIndex(e => e.KeyHash, "IX_Admin_KeyHash").IsUnique();
		});
	}
}