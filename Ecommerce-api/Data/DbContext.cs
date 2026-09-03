using Ecommerce_api.Domain;
using Microsoft.EntityFrameworkCore;
using File = Ecommerce_api.Domain.File;

namespace Ecommerce_api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<File> Files => Set<File>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
        
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Categories)
            .WithMany()
            .UsingEntity(j => j.ToTable("product_categories"));
        
        
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Files)
            .WithOne(f => f.Product)
            .HasForeignKey(f => f.ProductId)
            .IsRequired();
        
        modelBuilder.Entity<File>()
            .HasOne(f => f.Product)
            .WithMany(p => p.Files)
            .HasForeignKey(f => f.ProductId)
            .IsRequired();
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries<IAuditable>()
            .Where(e => e.State == EntityState.Modified || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}