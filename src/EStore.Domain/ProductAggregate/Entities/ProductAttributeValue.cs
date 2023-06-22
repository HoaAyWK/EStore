using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Models;

namespace EStore.Domain.ProductAggregate.Entities;

public sealed class ProductAttributeValue : Entity<ProductAttributeValueId>
{
    public string Name { get; private set; } = null!;

    public string? Alias { get; private set; }

    public decimal PriceAdjustment { get; private set; }

    public string? RawCombinedAttributes { get; private set; }

    private ProductAttributeValue()
    {
    }

    private ProductAttributeValue(
        ProductAttributeValueId id,
        string name,
        string? alias,
        decimal priceAdjustment) : base(id)
    {
        Name = name;
        Alias = alias;
        PriceAdjustment = priceAdjustment;
    }

    public static ProductAttributeValue Create(
        string name,
        decimal priceAdjustment,
        string? alias = null)
    {
        return new(
            ProductAttributeValueId.CreateUnique(),
            name,
            alias,
            priceAdjustment);
    }

    public void UpdateRawConnectedAttributes(string? rawConnectedAttributes)
    {
        RawCombinedAttributes = rawConnectedAttributes;
    }

    public void UpdateDetails(
        string name,
        string? alias,
        decimal priceAdjustment)
    {
        Name = name;
        Alias = alias;
        PriceAdjustment = priceAdjustment;
    }
}
