using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate;
using EStore.Domain.ProductVariantAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public class ProductVariantConfigurations : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariants");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("ProductVariantId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ProductVariantId.Create(value));

        builder.Property(p => p.ProductId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ProductId.Create(value));

        builder.Property(v => v.Price)
            .HasColumnType("decimal(18, 2)");

        builder.HasIndex(p => p.Id);

        builder.HasIndex(p => p.ProductId);
    }
}
