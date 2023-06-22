using EStore.Domain.CartAggregate;
using EStore.Domain.CartAggregate.Entities;
using EStore.Domain.CartAggregate.ValueObjects;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public class CartConfigurations : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        ConfigureCartsTable(builder);
        ConfigureCartItemsTable(builder);
    }
    private void ConfigureCartsTable(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("CartId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CartId.Create(value));

        builder.Property(c => c.CustomerId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CustomerId.Create(value));

        builder.Ignore(c => c.TotalItems);
    }

    private void ConfigureCartItemsTable(EntityTypeBuilder<Cart> builder)
    {
        builder.OwnsMany(c => c.Items, ib =>
        {
            ib.ToTable("CartItems");

            ib.WithOwner().HasForeignKey("CartId");

            ib.HasKey(nameof(CartItem.Id), "CartId");

            ib.Property(i => i.Id)
                .HasColumnName("CartItemId")
                .HasConversion(
                    id => id.Value,
                    value => CartItemId.Create(value));
            
            ib.Property(i => i.ProductVariantId)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id!.Value,
                    value => ProductVariantId.Create(value));

            ib.Property(i => i.ProductId)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ProductId.Create(value));

            ib.Property(i => i.UnitPrice)
                .HasColumnType("decimal(18, 2)");
        });

        builder.Metadata.FindNavigation(nameof(Cart.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
