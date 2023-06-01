using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        ConfigureProductTable(builder);
        ConfigureProductImageTable(builder);
        ConfigureProductVariantAttributeIdsTable(builder);
        ConfigureProductVariantAttributeCombinationTable(builder);
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


    private void ConfigureProductVariantAttributeIdsTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(p => p.ProductVariantAttributes, pvab =>
        {
            pvab.ToTable("ProductVariantAttributes");

            pvab.WithOwner().HasForeignKey("ProductId");

            pvab.HasKey(nameof(ProductVariantAttribute.Id), "ProductId");

            pvab.Property(pva => pva.Id)
                .HasColumnName("ProductVariantAttributeId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ProductVariantAttributeId.Create(value));
            
            pvab.Property(pva => pva.ProductAttributeId)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ProductAttributeId.Create(value));

            pvab.OwnsMany(pva => pva.ProductVariantAttributeValues, pvavb =>
            {
                pvavb.ToTable("ProductVariantAttributeValues");

                pvavb.WithOwner().HasForeignKey("ProductVariantAttributeId", "ProductId");

                pvavb.HasKey(nameof(ProductVariantAttributeValue.Id), "ProductVariantAttributeId", "ProductId");

                pvavb.Property(pvav => pvav.Id)
                    .HasColumnName("ProductVariantAttributeValueId")
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => ProductVariantAttributeValueId.Create(value));

                pvavb.Property(pvav => pvav.Name)
                    .HasMaxLength(100);

                pvavb.Property(pvav => pvav.Alias)
                    .HasMaxLength(30);

                pvavb.Property(pvav => pvav.PriceAdjustment)
                    .HasColumnType("decimal(18, 2)");
            });

            pvab.Navigation(pva => pva.ProductVariantAttributeValues)
                .Metadata.SetField("_productVariantAttributeValues");

            pvab.Navigation(pva => pva.ProductVariantAttributeValues)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Metadata.FindNavigation(nameof(Product.ProductVariantAttributes))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureProductVariantAttributeCombinationTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(p => p.ProductVariantAttributeCombinations, pvacb =>
        {
            pvacb.ToTable("ProductVariantAttributeCombinations");

            pvacb.WithOwner().HasForeignKey("ProductId");

            pvacb.HasKey(nameof(ProductVariantAttributeCombination.Id), "ProductId");

            pvacb.Property(pvac => pvac.Id)
                .HasColumnName("ProductVariantAttributeCombinationId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ProductVariantAttributeCombinationId.Create(value));
            
            pvacb.Property(pvac => pvac.Price)
                .HasColumnType("decimal(18, 2)");

            pvacb.OwnsMany(pvac => pvac.ProductVariantAttributeSelections, pvasb =>
            {
                pvasb.ToTable("ProductVariantAttributeSelections");

                pvasb.WithOwner().HasForeignKey("ProductVariantAttributeCombinationId", "ProductId");

                pvasb.HasKey(nameof(ProductVariantAttributeSelection.Id), "ProductVariantAttributeCombinationId", "ProductId");

                pvasb.Property(pvas => pvas.Id)
                    .HasColumnName("ProductVariantAttributeSelectionId")
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => ProductVariantAttributeSelectionId.Create(value));

                pvasb.Property(pvas => pvas.ProductVariantAttributeId)
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => ProductVariantAttributeId.Create(value));

                pvasb.Property(pvas => pvas.ProductVariantAttributeValueId)
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => ProductVariantAttributeValueId.Create(value));
            });


            pvacb.Navigation(pvac => pvac.ProductVariantAttributeSelections)
                .Metadata.SetField("_productVariantAttributeSelections");

            pvacb.Navigation(pvac => pvac.ProductVariantAttributeSelections)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}
