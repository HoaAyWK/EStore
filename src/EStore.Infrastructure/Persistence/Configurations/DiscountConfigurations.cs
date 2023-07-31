using EStore.Domain.DiscountAggregate;
using EStore.Domain.DiscountAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public sealed class DiscountConfigurations : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        ConfigureDiscountsTable(builder);
    }

    private void ConfigureDiscountsTable(EntityTypeBuilder<Discount> builder)
    {
        builder.ToTable("Discounts");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("DiscountId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => DiscountId.Create(value));

        builder.Property(d => d.Name)
            .HasMaxLength(Discount.MaxNameLength);

        builder.Property(d => d.DiscountPercentage)
            .HasColumnType("decimal(18,2)");

        builder.Property(d => d.DiscountAmount)
            .HasColumnType("decimal(18,2)");
    }
}
