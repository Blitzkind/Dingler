using Dingler.Data.Entities.HexGame;
using Microsoft.EntityFrameworkCore;
using MatchType = Dingler.Data.Entities.HexGame.MatchType;

namespace Dingler.Data.Context;

public partial class HexGameContext : DbContext
{
	public HexGameContext()
	{ }
	
	public virtual DbSet<GameDump> GameDumps { get; set; }
	public virtual DbSet<Game> GameInfos { get; set; }
	public virtual DbSet<GameLog> GameLogs { get; set; }
	public virtual DbSet<GamePlayer> GamePlayers { get; set; }
	public virtual DbSet<TournamentPlayer> TournamentPlayers { get; set; }
	public virtual DbSet<TournamentState> TournamentStates { get; set; }
	public virtual DbSet<Tournament> Tournaments { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlite("Data Source=data/game.db");
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DraftSet>(entity =>
		{
			entity.ToTable("DraftSets");
			entity.HasKey(e => e.Id);

			entity.HasOne(ds => ds.Tournament)
				.WithMany(p => p.DraftSets)
				.HasForeignKey(ds => ds.TournamentId);
            
			entity.HasOne(ds => ds.CardSet)
				.WithMany(p => p.DraftSets)
				.HasForeignKey(ds => ds.CardSetId);
		});

		modelBuilder.Entity<Tournament>(entity =>
		{
			entity.ToTable("Tournaments");
			entity.HasKey(e => e.Id);

			entity.HasIndex(e => e.Description, "IX_Tournament_Description").IsUnique();

			entity.HasOne(e => e.StartCondition)
				.WithMany(e => e.Tournaments)
				.HasForeignKey(e => e.StartConditionId)
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(e => e.TournamentType)
				.WithMany(e => e.Tournaments)
				.HasForeignKey(e => e.TournamentTypeId)
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(e => e.MatchType)
				.WithMany(e => e.Tournaments)
				.HasForeignKey(e => e.MatchTypeId)
				.OnDelete(DeleteBehavior.Restrict);
		});
		
		modelBuilder.Entity<TournamentType>(entity =>
		{
			entity.ToTable("TournamentType");
			entity.HasKey(e => e.Id);
            
			entity.HasIndex(e => e.Name, "IX_TournamentType_Name").IsUnique();
		});
		
		modelBuilder.Entity<StartCondition>(entity =>
		{
			entity.ToTable("StartCondition");
			entity.HasKey(e => e.Id);

			entity.HasIndex(e => e.Description, "IX_StartCondition_Description").IsUnique();
		});
		
		modelBuilder.Entity<MatchType>(entity =>
		{
			entity.ToTable("MatchType");
			entity.HasKey(e => e.Id);
            
			entity.HasIndex(e => e.Description, "IX_MatchType_Description").IsUnique();
		});
		
		modelBuilder.Entity<CardSet>(entity =>
		{
			entity.ToTable("Set");
			entity.HasKey(e => e.Id);

			entity.HasIndex(e => e.Name, "IX_Set_Name").IsUnique();
		});

		modelBuilder.Entity<GameState>().HasData(
			new GameState { Id = 1, Description = "WAITING" },
			new GameState { Id = 2, Description = "RUNNING" },
			new GameState { Id = 3, Description = "FINISHED" }
		);

		modelBuilder.Entity<TournamentState>().HasData(
			new TournamentState { Id = 1, Description = "WAITING" },
			new TournamentState { Id = 2, Description = "RUNNING" },
			new TournamentState { Id = 3, Description = "FINISHED" }
			);
	}
}