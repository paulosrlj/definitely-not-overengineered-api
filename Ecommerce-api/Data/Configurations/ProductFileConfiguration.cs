using Ecommerce_api.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_api.Data.Configurations;

public class ProductFileConfiguration : IEntityTypeConfiguration<ProductFile>
{
    public void Configure(EntityTypeBuilder<ProductFile> builder)
    {
        builder.ToTable("product_files");

        builder.HasKey(pf => new { pf.ProductId, pf.FileId });
        
        // ProductFile n <--- 1 Product
        builder
            .HasOne(pf => pf.Product)
            .WithMany(p => p.Files)
            .HasForeignKey(pf => pf.ProductId)
            .IsRequired();

        // ProductFile n <--- 1 File
        builder.HasOne(pf => pf.File)
            .WithMany()
            .HasForeignKey(pf => pf.FileId)
            .IsRequired();
        
        builder.Property(user => user.CreatedAt)
            .IsRequired();

        builder.Property(user => user.UpdatedAt)
            .IsRequired();
    }
}