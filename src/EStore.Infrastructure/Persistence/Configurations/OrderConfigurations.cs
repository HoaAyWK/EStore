using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Entities;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.ValueObjects;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public sealed class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        ConfigureOrdersTable(builder);
        ConfigureOrderItemsTable(builder);
    }

    private void ConfigureOrdersTable(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("OrderId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value));

        builder.Property(o => o.CustomerId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CustomerId.Create(value));
        
        builder.OwnsOne(o => o.ShippingAddress);

        builder.Property(o => o.OrderStatus)
            .HasConversion<int>(
                status => status.Value,
                value => OrderStatus.FromValue(value)!);

        builder.Ignore(o => o.TotalAmount);
    }

    private void ConfigureOrderItemsTable(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(o => o.OrderItems, ib =>
        {
            ib.ToTable("OrderItems");

            ib.WithOwner().HasForeignKey("OrderId");

            ib.HasKey(nameof(OrderItem.Id), "OrderId");

            ib.Property(i => i.Id)
                .HasColumnName("OrderItemId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => OrderItemId.Create(value));

            ib.Property(i => i.UnitPrice)
                .HasColumnType("decimal(18, 2)");

            ib.OwnsOne(i => i.ItemOrdered, iob =>
            {
                iob.Property(io => io.ProductId)
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => ProductId.Create(value));

                iob.Property(io => io.ProductVariantId)
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id!.Value,
                        value => ProductVariantId.Create(value));

                iob.Property(io => io.ProductName)
                    .HasMaxLength(Product.MaxNameLength);
            });
        });

        builder.Metadata.FindNavigation(nameof(Order.OrderItems))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
