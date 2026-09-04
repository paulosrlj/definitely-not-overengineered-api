using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = Ecommerce_api.Domain.File;

namespace Ecommerce_api.Data.Configurations;

public class FileConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.ToTable("files");

        builder.HasKey(file => file.Id);

        builder.Property(file => file.FileName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(file => file.ContentType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(file => file.Size)
            .IsRequired();

        builder.Property(file => file.Provider)
            .HasConversion<string>()
            .IsRequired();
        
        builder.Property(file => file.StorageKey)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(user => user.CreatedAt)
            .IsRequired();

        builder.Property(user => user.UpdatedAt)
            .IsRequired();
    }

}