using EStore.Domain.BannerAggregate;
using EStore.Domain.BannerAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.BannerAggregate.Enumerations;

namespace EStore.Infrastructure.Persistence.Configurations;

public class BannerConfigurations : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        ConfigureBannersTable(builder);
    }

    private void ConfigureBannersTable(EntityTypeBuilder<Banner> builder)
    {
        builder.ToTable("Banners");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("BannerId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => BannerId.Create(value));

        builder.Property(b => b.ProductId)
            .HasColumnName(nameof(ProductId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ProductId.Create(value));

        builder.Property(b => b.ProductVariantId)
            .HasColumnName(nameof(ProductVariantId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id!.Value,
                value => ProductVariantId.Create(value));

        builder.Property(b => b.Direction)
            .HasConversion<int>(
                direction => direction.Value,
                value => BannerDirection.FromValue(value)!);

        builder.HasQueryFilter(p => !p.Deleted);
    }
}
