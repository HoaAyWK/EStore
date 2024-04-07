using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.DiscountAggregate.ValueObjects;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Entities;
using EStore.Domain.ProductAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        ConfigureProductsTable(builder);
        ConfigureProductImagesTable(builder);
        ConfigureProductAttributesTable(builder);
        ConfigureProductVariantsTable(builder);
        ConfigureProductReviewsTable(builder);
    }

    private void ConfigureProductsTable(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("ProductId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ProductId.Create(value));

        builder.Property(p => p.Name)
            .HasMaxLength(Product.MaxNameLength);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.BrandId)
            .HasColumnName("ProductBrandId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => BrandId.Create(value));

        builder.Property(p => p.CategoryId)
            .HasColumnName("ProductCategoryId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CategoryId.Create(value));

        builder.Property(p => p.DiscountId)
            .HasColumnName("ProductDiscountId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id!.Value,
                value => DiscountId.Create(value));

        builder.OwnsOne(p => p.AverageRating);

        builder.HasQueryFilter(p => !p.Deleted);
    }

    private void ConfigureProductImagesTable(EntityTypeBuilder<Product> builder)
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

    private void ConfigureProductAttributesTable(EntityTypeBuilder<Product> builder)
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

                pavb.Property(av => av.Color)
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

    private void ConfigureProductVariantsTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(p => p.ProductVariants, vb =>
        {
            vb.ToTable("ProductVariants");

            vb.WithOwner().HasForeignKey("ProductId");

            vb.HasKey(nameof(ProductVariant.Id), "ProductId");

            builder.Property(v => v.Price)
                .HasColumnType("decimal(18, 2)");

            vb.Property(v => v.Id)
                .HasColumnName("ProductVariantId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ProductVariantId.Create(value));
        });

        builder.Metadata.FindNavigation(nameof(Product.ProductVariants))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureProductReviewsTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(p => p.ProductReviews, rb =>
        {
            rb.ToTable("ProductReviews");

            rb.WithOwner().HasForeignKey("ProductId");

            rb.HasKey(nameof(ProductReview.Id), "ProductId");

            rb.Property(r => r.Id)
                .HasColumnName("ProductReviewId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ProductReviewId.Create(value));

            rb.Property(r => r.Title)
                .HasMaxLength(ProductReview.MinTitleLength);

            rb.Property(r => r.Content)
                .HasMaxLength(ProductReview.MaxContentLength);

            rb.Property(r => r.OwnerId)
                .HasColumnName("ProductReviewOwnerId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => CustomerId.Create(value));

            rb.OwnsMany(r => r.ReviewComments, rcb =>
            {
                rcb.ToTable("ProductReviewComments");

                rcb.WithOwner().HasForeignKey("ProductReviewId", "ProductId");

                rcb.HasKey(nameof(ProductReviewComment.Id), "ProductReviewId", "ProductId");

                rcb.Property(rc => rc.Id)
                    .HasColumnName("ProductReviewCommentId")
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => ProductReviewCommentId.Create(value));

                rcb.Property(rc => rc.OwnerId)
                    .HasColumnName("ProductReviewCommentOwnerId")
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => CustomerId.Create(value));

                rcb.Property(rc => rc.Content)
                    .HasMaxLength(ProductReviewComment.MinContentLength);

                rcb.Property(c => c.ParentId)
                    .HasColumnName("ProductReviewCommentParentId")
                    .ValueGeneratedNever()
                    .HasConversion(
                        parentId => parentId!.Value,
                        value =>  ProductReviewCommentId.Create(value));             
            });

            rb.Navigation(rc => rc.ReviewComments)
                    .Metadata.SetField("_reviewComments");

            rb.Navigation(pva => pva.ReviewComments)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Metadata.FindNavigation(nameof(Product.ProductReviews))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
