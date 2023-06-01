using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.ProductAggregate;

public sealed class ProductVariantAttributeSelection : Entity<ProductVariantAttributeSelectionId>
{
    public ProductVariantAttributeId ProductVariantAttributeId { get; private set; } = null!;
    
    public ProductVariantAttributeValueId ProductVariantAttributeValueId { get; private set; } = null!;

    private ProductVariantAttributeSelection()
    {
    }

    private ProductVariantAttributeSelection(
        ProductVariantAttributeSelectionId id,
        ProductVariantAttributeId productVariantAttributeId,
        ProductVariantAttributeValueId productVariantAttributeValueId)
        : base(id)
    {
        ProductVariantAttributeId = productVariantAttributeId;
        ProductVariantAttributeValueId = productVariantAttributeValueId;
    }

    public static ProductVariantAttributeSelection Create(
        ProductVariantAttributeId productVariantAttributeId,
        ProductVariantAttributeValueId productVariantAttributeValueId)
    {
        return new(
            ProductVariantAttributeSelectionId.CreateUnique(),
            productVariantAttributeId,
            productVariantAttributeValueId);
    }
}
