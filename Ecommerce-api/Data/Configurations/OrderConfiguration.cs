using Ecommerce_api.Domain;
using Ecommerce_api.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_api.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {   
        builder.ToTable("orders");

        builder.HasKey(order => order.Id);

       builder.Property(order => order.Total)
            .IsRequired();

        builder.Property(order => order.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(order => order.StripeSessionId)
            .HasMaxLength(255);

        builder.HasIndex(order => order.StripeSessionId)
            .IsUnique();

        builder.Property(order => order.CreatedAt)
            .IsRequired();

        builder.Property(order => order.UpdatedAt)
            .IsRequired();

        // Order -> OrderItem
        builder.HasMany(order => order.Items)
            .WithOne(item => item.Order)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Address
        builder.ComplexProperty(order => order.Address, address =>
        {
            address.Property(a => a.Street)
                .HasColumnName("street")
                .IsRequired()
                .HasMaxLength(200);

            address.Property(a => a.Number)
                .HasColumnName("number")
                .IsRequired();

            address.Property(a => a.Apartment)
                .HasColumnName("apartment")
                .HasMaxLength(50);

            address.Property(a => a.City)
                .HasColumnName("city")
                .IsRequired()
                .HasMaxLength(100);

            address.Property(a => a.State)
                .HasColumnName("state")
                .IsRequired()
                .HasMaxLength(100);

            address.Property(a => a.ZipCode)
                .HasColumnName("zip_code")
                .IsRequired()
                .HasMaxLength(20);

            address.Property(a => a.Country)
                .HasColumnName("country")
                .IsRequired()
                .HasMaxLength(100);

            address.Property(a => a.Phone)
                .HasColumnName("phone")
                .IsRequired()
                .HasMaxLength(30);

            address.Property(a => a.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(320);
        });
    }

}