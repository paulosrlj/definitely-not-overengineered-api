using Ecommerce_api.Models;
using Microsoft.EntityFrameworkCore;
    
namespace Ecommerce_api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");              // @@map("users")
            entity.HasKey(e => e.Id);
            entity.HasIndex(u => u.Email).IsUnique(); // @unique

            entity.Property(u => u.Role)
                .HasConversion<string>();          // salva o enum como texto no banco
        });
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<User>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}