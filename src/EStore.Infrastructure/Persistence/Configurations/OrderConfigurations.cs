using EStore.Domain.CustomerAggregate.Entities;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Entities;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.ValueObjects;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public sealed class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        ConfigureOrdersTable(builder);
        ConfigureOrderItemsTable(builder);
        ConfigureOrderStatusHistoryTrackingsTable(builder);
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
        
        builder.OwnsOne(o => o.ShippingAddress, sb =>
        {
            sb.Property(sa => sa.ReceiverName)
                .HasMaxLength(Address.MaxReceiverNameLength);

            sb.Property(sa => sa.PhoneNumber)
                .HasMaxLength(Address.MaxPhoneNumberLength);

            sb.Property(sa => sa.Street)
                .HasMaxLength(Address.MaxStreetLength);

            sb.Property(sa => sa.City)
                .HasMaxLength(Address.MaxCityLength);

            sb.Property(sa => sa.State)
                .HasMaxLength(Address.MaxStateLength);

            sb.Property(sa => sa.Country)
                .HasMaxLength(Address.MaxCountryLength);

            sb.Property(sa => sa.ZipCode)
                .HasMaxLength(Address.MaxZipCodeLength);
        });

        builder.Property(o => o.OrderStatus)
            .HasConversion<int>(
                status => status.Value,
                value => OrderStatus.FromValue(value)!);

        builder.Property(o => o.PaymentMethod)
            .HasConversion<int>(
                method => method.Value,
                value => PaymentMethod.FromValue(value)!);

        builder.Property(o => o.PaymentStatus)
            .HasConversion<int>(
                status => status.Value,
                value => PaymentStatus.FromValue(value)!);

        builder.Ignore(o => o.TotalAmount);

        builder.HasQueryFilter(o => !o.Deleted);
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

            ib.Property(i => i.DiscountAmount)
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

    private void ConfigureOrderStatusHistoryTrackingsTable(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(o => o.OrderStatusHistoryTrackings, ob =>
        {
            ob.ToTable("OrderStatusHistoryTrackings");

            ob.WithOwner().HasForeignKey("OrderId");

            ob.HasKey(nameof(OrderStatusHistoryTracking.Id), "OrderId");

            ob.Property(i => i.Id)
                .HasColumnName("OrderStatusHistoryTrackingId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => OrderStatusHistoryTrackingId.Create(value));

            ob.Property(i => i.Status)
                .HasConversion<int>(
                    status => status.Value,
                    value => OrderStatusHistory.FromValue(value)!);
        });
    }
}
