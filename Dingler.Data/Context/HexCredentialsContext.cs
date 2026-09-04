using Dingler.Data.Entities.Credentials;
using Microsoft.EntityFrameworkCore;

namespace Dingler.Data.Context;

public partial class HexCredentialsContext : DbContext
{
    public HexCredentialsContext()
    {
    }

    public HexCredentialsContext(DbContextOptions<HexCredentialsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BannableOffense> BannableOffenses { get; set; }

    public virtual DbSet<BannedUser> BannedUsers { get; set; }

    public virtual DbSet<UserCredential> UserCredentials { get; set; }

    public virtual DbSet<UserLoginAttempt> UserLoginAttempts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=data/hexCredentials.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BannableOffense>(entity =>
        {
            entity.HasIndex(e => e.Offense, "IX_BannableOffenses_Offense").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<BannedUser>(entity =>
        {
            entity.HasIndex(e => e.UserCredentialsId, "IX_BannedUsers_UserCredentialsId").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Offense).WithMany(p => p.BannedUsers)
                .HasForeignKey(d => d.OffenseId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.UserCredentials).WithOne(p => p.BannedUser)
                .HasForeignKey<BannedUser>(d => d.UserCredentialsId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<UserCredential>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_UserCredentials_Email").IsUnique();
        });

        modelBuilder.Entity<UserLoginAttempt>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_UserLoginAttempts_UserId").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithOne(p => p.UserLoginAttempt).HasForeignKey<UserLoginAttempt>(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
