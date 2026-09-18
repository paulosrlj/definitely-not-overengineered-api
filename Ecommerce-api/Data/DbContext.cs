using Ecommerce_api.Domain;
using Ecommerce_api.Domain.Orders;
using Ecommerce_api.Domain.Payments;
using Ecommerce_api.Domain.Products;
using Microsoft.EntityFrameworkCore;
using File = Ecommerce_api.Domain.File;

namespace Ecommerce_api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<File> Files => Set<File>();
    public DbSet<ProductFile> ProductFiles => Set<ProductFile>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
    
    private void UpdateAuditFields()
    {
        var entries = ChangeTracker
            .Entries<IAuditable>()
            .Where(e =>
                e.State == EntityState.Added ||
                e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();

        return base.SaveChangesAsync(cancellationToken);
    }
}