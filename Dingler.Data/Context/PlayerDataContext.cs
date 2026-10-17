using Dingler.Data.Entities.PlayerData;
using Microsoft.EntityFrameworkCore;

namespace Dingler.Data.Context;

public partial class PlayerDataContext : DbContext
{
    public PlayerDataContext()
    {
    }

    public PlayerDataContext(DbContextOptions<PlayerDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Deck> Decks { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<PlayerProfile> PlayerProfiles { get; set; }

    public virtual DbSet<DraftCollection> DraftCollections { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=data/gameData.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Accounts_Email").IsUnique();
            
            entity.HasOne(a => a.PlayerProfile)
                .WithOne(p => p.Account)
                .HasForeignKey<PlayerProfile>(a => a.AccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Deck>(entity =>
        {
            entity.Property(d => d.DeckGuid)
                .HasConversion<string>()
                .HasColumnType("TEXT");

            entity.Property(d => d.ChampionGuid)
                .HasConversion<string>()
                .HasColumnType("TEXT");

            entity.HasIndex(e => e.DeckGuid, "IX_Decks_DeckGuid").IsUnique();

            entity.HasIndex(e => new { e.DeckName, e.PlayerProfileId }, "IX_Decks_DeckName_PlayerProfileId").IsUnique();

            entity.Property(e => e.ChampionGuid).HasDefaultValue(Guid.Empty);

            entity.HasOne(d => d.PlayerProfile).WithMany(p => p.Decks)
                .HasForeignKey(d => d.PlayerProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasIndex(e => new { e.RequesterId, e.RequestedId }, "IX_Friends_RequesterId_RequestedId").IsUnique();
            
            entity.HasOne(d => d.Requested)
                .WithMany(p => p.FriendRequesteds)
                .HasForeignKey(d => d.RequestedId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Requester)
                .WithMany(p => p.FriendRequesters)
                .HasForeignKey(d => d.RequesterId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            
            entity.HasOne(e => e.Status)
                .WithMany(e => e.Friends)
                .HasForeignKey(e => e.FriendStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<FriendStatus>(entity =>
        {
            entity.ToTable("FriendStatus");
            entity.HasKey(e => e.Id);
            
            entity.HasIndex(e => e.Description, "IX_FriendStatus_Description").IsUnique();

            var friendStatusLookup = Enum.GetValues(typeof(Enums.FriendStatus)).Cast<Enums.FriendStatus>()
                .OrderBy(v => v).Select(value => new FriendStatus() { Id = (int)value, Description = value.ToString() })
                .ToList();
            
            entity.HasData(friendStatusLookup);
        });
        
        modelBuilder.Entity<PlayerProfile>(entity =>
        {
            entity.HasIndex(e => e.Username, "IX_PlayerProfiles_Username").IsUnique();
    
            entity.Property(e => e.Elo)
                .HasDefaultValue(1000)
                .HasColumnName("ELO");
    
            entity.HasOne(e => e.Rank)
                .WithMany(e => e.PlayerProfiles)
                .HasForeignKey(e => e.RankId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.ToTable("Rank");
            entity.HasKey(e => e.Id);
            
            entity.HasIndex(e => e.Name, "IX_Rank_Name").IsUnique();

            var rankLookups = Enum.GetValues(typeof(Enums.Rank)).Cast<Enums.Rank>().OrderBy(v => v).Select(value =>
                new Rank() { Id = (int)value, Name = value.ToString() }).ToList();
            
            entity.HasData(rankLookups);
        });

        modelBuilder.Entity<DraftCollection>(entity =>
        {
            entity.ToTable("DraftCollections");
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.TournamentId, e.PlayerProfileId },
                    "IX_DraftCollections_TournamentId_PlayerProfileId")
                .IsUnique();
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
