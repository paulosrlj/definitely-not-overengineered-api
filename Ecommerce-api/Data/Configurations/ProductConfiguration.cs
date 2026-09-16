using Ecommerce_api.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;

namespace Ecommerce_api.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(product => product.Id);

        builder.Property(user => user.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(product => product.Description)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(product => product.Price)
            .IsRequired();

        builder.Property(product => product.Stock)
            .IsRequired();

        builder.Property(user => user.CreatedAt)
            .IsRequired();

        builder.Property(user => user.UpdatedAt)
            .IsRequired();

        builder.HasMany(product => product.OrderItems)
            .WithOne(item => item.Product)
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(p => p.Categories)
            .WithMany()
            .UsingEntity(j => j.ToTable("product_categories"));

        builder.Property<NpgsqlTsVector>("SearchVector")
            .HasComputedColumnSql(
                "to_tsvector('portuguese', coalesce(\"Name\", '') || ' ' || coalesce(\"Description\", ''))",
                stored: true
            );

        builder.HasIndex("SearchVector")
            .HasMethod("GIN");
    }
}