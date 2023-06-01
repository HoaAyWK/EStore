using EStore.Domain.Catalog.ProductAttributeAggregate;
using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public class ProductAttributeConfigurations : IEntityTypeConfiguration<ProductAttribute>
{
    public void Configure(EntityTypeBuilder<ProductAttribute> builder)
    {
        ConfigureProductAttributeTable(builder);
        ConfigureProductAttributeOptionSetTable(builder);
    }

    private void ConfigureProductAttributeTable(EntityTypeBuilder<ProductAttribute> builder)
    {
        builder.ToTable("ProductAttributes");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("ProductAttributeId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ProductAttributeId.Create(value));
        
        builder.Property(a => a.Name)
            .HasMaxLength(100);

        builder.Property(a => a.Alias)
            .HasMaxLength(30);
    }

    private void ConfigureProductAttributeOptionSetTable(EntityTypeBuilder<ProductAttribute> builder)
    {
        builder.OwnsMany(a => a.ProductAttributeOptionSets, osb => {
            
            osb.ToTable("ProductAttributeOptionSets");

            osb.WithOwner().HasForeignKey("ProductAttributeId");

            osb.HasKey("Id", "ProductAttributeId");

            osb.Property(os => os.Id)
                .HasColumnName("ProductAttributeOptionSetId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ProductAttributeOptionSetId.Create(value));
            
            osb.Property(os => os.Name)
                .HasMaxLength(100);

            osb.OwnsMany(os => os.ProductAttributeOptions, ob => {
                
                ob.ToTable("ProductAttributeOptions");

                ob.WithOwner()
                    .HasForeignKey("ProductAttributeOptionSetId", "ProductAttributeId");

                ob.HasKey(
                    nameof(ProductAttributeOption.Id),
                    "ProductAttributeOptionSetId",
                    "ProductAttributeId");

                ob.Property(o => o.Id)
                    .HasColumnName("ProductAttributeOptionId")
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => ProductAttributeOptionId.Create(value));
                
                ob.Property(o => o.Name)
                    .HasMaxLength(100);
                
                ob.Property(o => o.Alias)
                    .HasMaxLength(30);

                ob.Property(o => o.PriceAdjustment)
                    .HasColumnType("decimal(18, 2)");
            });

            osb.Navigation(os => os.ProductAttributeOptions)
                .Metadata.SetField("_productAttributeOptions");

            osb.Navigation(os => os.ProductAttributeOptions)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

        });

        builder.Metadata.FindNavigation(nameof(ProductAttribute.ProductAttributeOptionSets))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
