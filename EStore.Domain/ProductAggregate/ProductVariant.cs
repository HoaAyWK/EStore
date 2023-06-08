using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Models;

namespace EStore.Domain.ProductAggregate;

public sealed class ProductVariant : Entity<ProductVariantId>
{
    private ProductAttributeSelection? _attributeSelection;
    private string? _rawAttributes;

    public decimal? Price { get; private set; }

    public int StockQuantity { get; private set; }

    public bool IsActive { get; private set; }

    public string AssignedProductImageIds { get; private set; } = null!;

    public string? RawAttributes
    {
        get => _rawAttributes;

        set
        {
            _rawAttributes = value;
            _attributeSelection = null;
        }
    }

    public ProductAttributeSelection AttributeSelection
        => _attributeSelection ??= ProductAttributeSelection.Create(RawAttributes);

    private ProductVariant()
    {
    }

    private ProductVariant(
        ProductVariantId id,
        decimal? price,
        int stockQuantity,
        bool isActive,
        string assignedProductImageIds)
        : base(id)
    {
        Price = price;
        StockQuantity = stockQuantity;
        IsActive = isActive;
        AssignedProductImageIds = assignedProductImageIds;
    }

    public static ProductVariant Create(
        int stockQuantity,
        decimal? price,
        string assignedProductImageIds,
        bool isActive = true)
    {
        return new(
            ProductVariantId.CreateUnique(),
            price,
            stockQuantity,
            isActive,
            assignedProductImageIds);
    }

    public void UpdateRawAttributes(string? rawAttributes)
    {
        _rawAttributes = rawAttributes;
    }

    public void UpdateDetails(int stockQuantity, decimal? price, bool isActive)
    {
        StockQuantity = stockQuantity;
        Price = price;
        IsActive = isActive;
    }

    public void UpdateImages(string assignedImageIds)
    {
        AssignedProductImageIds = assignedImageIds;
    }
}