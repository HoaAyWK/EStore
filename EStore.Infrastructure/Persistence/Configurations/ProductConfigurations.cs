using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        ConfigureProductTable(builder);
        ConfigureProductImageTable(builder);
        ConfigureProductAttributeTable(builder);
        ConfigureProductVariantTable(builder);
    }

    private void ConfigureProductTable(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(100);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.SpecialPrice)
            .HasColumnType("decimal(18, 2)");

        builder.OwnsOne(p => p.AverageRating);

        builder.HasOne(p => p.Brand)
            .WithMany()
            .HasForeignKey(p => p.BrandId);

        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId);

        builder.Property(p => p.Id)
            .HasColumnName("ProductId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ProductId.Create(value));
    }

    private void ConfigureProductImageTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(p => p.Images, ib =>
        {
            ib.ToTable("ProductImages");

            ib.WithOwner().HasForeignKey("ProductId");

            ib.HasKey("Id", "ProductId");

            ib.Property(i => i.Id)
                .HasColumnName("ProductImageId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ProductImageId.Create(value));
        });

        builder.Metadata.FindNavigation(nameof(Product.Images))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }


    private void ConfigureProductAttributeTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(p => p.ProductAttributes, pab =>
        {
            pab.ToTable("ProductAttributes");

            pab.WithOwner().HasForeignKey("ProductId");

            pab.HasKey(nameof(ProductAttribute.Id), "ProductId");

            pab.Property(pva => pva.Id)
                .HasColumnName("ProductAttributeId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ProductAttributeId.Create(value));

            pab.Property(pa => pa.Name)
                .HasMaxLength(100);

            pab.Property(pa => pa.Alias)
                .HasMaxLength(30);

            pab.OwnsMany(pva => pva.ProductAttributeValues, pavb =>
            {
                pavb.ToTable("ProductAttributeValues");

                pavb.WithOwner().HasForeignKey("ProductAttributeId", "ProductId");

                pavb.HasKey(nameof(ProductAttributeValue.Id), "ProductAttributeId", "ProductId");

                pavb.Property(av => av.Id)
                    .HasColumnName("ProductAttributeValueId")
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => ProductAttributeValueId.Create(value));

                pavb.Property(av => av.Name)
                    .HasMaxLength(100);

                pavb.Property(av => av.Alias)
                    .HasMaxLength(30);

                pavb.Property(av => av.PriceAdjustment)
                    .HasColumnType("decimal(18, 2)");
            });

            pab.Navigation(pva => pva.ProductAttributeValues)
                .Metadata.SetField("_productAttributeValues");

            pab.Navigation(pva => pva.ProductAttributeValues)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Metadata.FindNavigation(nameof(Product.ProductAttributes))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureProductVariantTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(p => p.ProductVariants, pvb =>
        {
            pvb.ToTable("ProductVariants");

            pvb.WithOwner().HasForeignKey("ProductId");

            pvb.HasKey(nameof(ProductVariant.Id), "ProductId");

            pvb.Property(v => v.Id)
                .HasColumnName("ProductVariantId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ProductVariantId.Create(value));

            pvb.Property(v => v.Price)
                .HasColumnType("decimal(18, 2)");

        });

        builder.Metadata.FindNavigation(nameof(Product.ProductVariants))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
