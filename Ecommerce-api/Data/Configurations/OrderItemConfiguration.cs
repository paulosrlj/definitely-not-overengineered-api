using Ecommerce_api.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_api.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {   
        builder.ToTable("orders_items");

        builder.HasKey(order => order.Id);

       builder.Property(order => order.Quantity)
            .IsRequired();

        builder.Property(order => order.UnitPrice)
            .IsRequired();

       builder.HasOne(orderItem => orderItem.Order)
           .WithMany(order => order.Items)
           .HasForeignKey(item => item.OrderId)
           .OnDelete(DeleteBehavior.Cascade);
       
       builder.HasOne(orderItem => orderItem.Product)
           .WithMany(product => product.OrderItems)
           .HasForeignKey(item => item.ProductId)
           .OnDelete(DeleteBehavior.Restrict);
       
       builder.HasIndex(item => new  { item.OrderId, item.ProductId }).IsUnique();
    }

}