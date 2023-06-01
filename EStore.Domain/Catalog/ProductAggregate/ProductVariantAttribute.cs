using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;
using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.ProductAggregate;

public sealed class ProductVariantAttribute : Entity<ProductVariantAttributeId>
{
    private readonly List<ProductVariantAttributeValue> _productVariantAttributeValues = new();

    public ProductAttributeId ProductAttributeId { get; private set; } = null!;

    public IReadOnlyList<ProductVariantAttributeValue> ProductVariantAttributeValues
        => _productVariantAttributeValues.AsReadOnly();

    private ProductVariantAttribute()
    {
    }

    private ProductVariantAttribute(
        ProductVariantAttributeId id,
        ProductAttributeId productAttributeId) : base(id)
    {
        ProductAttributeId = productAttributeId;
    }

    public static ProductVariantAttribute Create(
        ProductId productId,
        ProductAttributeId productAttributeId)
    {
        return new(
            ProductVariantAttributeId.CreateUnique(),
            productAttributeId);
    }
}
