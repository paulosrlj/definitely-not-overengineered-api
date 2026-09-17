using Ecommerce_api.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_api.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");
        
        builder.HasKey(payment => payment.Id);
        
        builder.Property(payment => payment.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(payment => payment.Provider)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(payment => payment.ExternalId);

        builder.Property(payment => payment.Total)
            .IsRequired();
        
        // Belongs to Order
        builder
            .HasOne(payment => payment.Order)
            .WithMany(order => order.Payments)
            .HasForeignKey(order => order.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

