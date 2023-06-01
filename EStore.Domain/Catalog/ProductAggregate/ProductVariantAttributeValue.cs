using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.ProductAggregate;

public sealed class ProductVariantAttributeValue : Entity<ProductVariantAttributeValueId>
{
    public string Name { get; private set; } = null!;

    public string? Alias { get; private set; }

    public decimal PriceAdjustment { get; private set; }

    private ProductVariantAttributeValue()
    {
    }

    private ProductVariantAttributeValue(
        ProductVariantAttributeValueId id,
        string name,
        string? alias,
        decimal priceAdjustment) : base(id)
    {
        Name = name;
        Alias = alias;
        PriceAdjustment = priceAdjustment;
    }

    public static ProductVariantAttributeValue Create(
        string name,
        decimal priceAdjustment,
        string? alias = null)
    {
        return new(
            ProductVariantAttributeValueId.CreateUnique(),
            name,
            alias,
            priceAdjustment);
    }
}
