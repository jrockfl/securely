using Microsoft.EntityFrameworkCore;

using Securely.Domain.Enities;

namespace Securely.Infrastructure.Repositories;
public class SecurelyDbContext : DbContext
{
    public SecurelyDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<SecureMessage> Messages { get; set; }

    public DbSet<EncryptionKey> EncryptionKeys { get; set; }

    public DbSet<SecureMessageHistory> SecureMessageHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SecureMessage>().ToTable("SecureMessage");
        modelBuilder.Entity<EncryptionKey>().ToTable("EncryptionKey");
        modelBuilder.Entity<SecureMessage>().HasIndex(sm => sm.EncryptionKeyId).IsUnique();
        modelBuilder.Entity<SecureMessage>().Property(e => e.CreatedUtc).HasDefaultValueSql("GETUTCDATE()");
        modelBuilder.Entity<SecureMessage>().Property(e => e.UpdatedUtc).HasDefaultValueSql("GETUTCDATE()");
        modelBuilder.Entity<SecureMessage>().Property(e => e.ExpirationUtc).HasDefaultValueSql("DATEADD(day, 1, GETDATE())");
    }
}
