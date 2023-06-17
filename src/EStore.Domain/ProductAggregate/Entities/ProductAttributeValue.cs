using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Models;

namespace EStore.Domain.ProductAggregate.Entities;

public sealed class ProductAttributeValue : Entity<ProductAttributeValueId>
{

    private string? _rawCombinedAttributes;
    private ProductAttributeSelection? _combinedAttributes;

    public string Name { get; private set; } = null!;

    public string? Alias { get; private set; }

    public decimal PriceAdjustment { get; private set; }

    public string? RawCombinedAttributes
    {
        get => _rawCombinedAttributes;

        set
        {
            _rawCombinedAttributes = value;
            _combinedAttributes = null;
        }
    }

    public ProductAttributeSelection CombinedAttributes =>
        _combinedAttributes ??= ProductAttributeSelection.Create(RawCombinedAttributes);

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
        _rawCombinedAttributes = rawConnectedAttributes;
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
