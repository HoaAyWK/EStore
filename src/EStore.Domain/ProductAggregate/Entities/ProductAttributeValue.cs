using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Models;

namespace EStore.Domain.ProductAggregate.Entities;

public sealed class ProductAttributeValue : Entity<ProductAttributeValueId>
{
    public string Name { get; private set; } = null!;

    public string? Color { get; private set; }

    public decimal PriceAdjustment { get; private set; }

    public string? RawCombinedAttributes { get; private set; }

    public int DisplayOrder { get; private set; }

    private ProductAttributeValue()
    {
    }

    private ProductAttributeValue(
        ProductAttributeValueId id,
        string name,
        string? color,
        decimal priceAdjustment,
        int displayOrder) : base(id)
    {
        Name = name;
        Color = color;
        PriceAdjustment = priceAdjustment;
        DisplayOrder = displayOrder;
    }

    public static ProductAttributeValue Create(
        string name,
        decimal priceAdjustment,
        string? color,
        int displayOrder)
    {
        return new(
            ProductAttributeValueId.CreateUnique(),
            name,
            color,
            priceAdjustment,
            displayOrder);
    }

    public void UpdateRawConnectedAttributes(string? rawConnectedAttributes)
    {
        RawCombinedAttributes = rawConnectedAttributes;
    }

    public void UpdateDetails(
        string name,
        string? color,
        decimal priceAdjustment,
        int displayOrder)
    {
        Name = name;
        Color = color;
        PriceAdjustment = priceAdjustment;
        DisplayOrder = displayOrder;
    }
}
