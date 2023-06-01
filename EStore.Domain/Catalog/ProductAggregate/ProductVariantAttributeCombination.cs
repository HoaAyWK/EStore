using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Models;

namespace EStore.Domain.Catalog.ProductAggregate;

public sealed class ProductVariantAttributeCombination : Entity<ProductVariantAttributeCombinationId>
{
    private readonly List<ProductVariantAttributeSelection> _productVariantAttributeSelections = new();

    public decimal? Price { get; private set; }

    public int StockQuantity { get; private set; }

    public bool IsActive { get; private set; }

    public string AssignedProductImageIds { get; private set; } = null!;

    public IReadOnlyList<ProductVariantAttributeSelection> ProductVariantAttributeSelections
        => _productVariantAttributeSelections.AsReadOnly();

    private ProductVariantAttributeCombination()
    {
    }

    private ProductVariantAttributeCombination(
        ProductVariantAttributeCombinationId id,
        decimal? price,
        int stockQuantity,
        bool isActive,
        string assignedProductImageIds,
        List<ProductVariantAttributeSelection> attributeSelections) : base(id)
    {
        Price = price;
        StockQuantity = stockQuantity;
        IsActive = isActive;
        AssignedProductImageIds = assignedProductImageIds;
        _productVariantAttributeSelections = attributeSelections;
    }

    public static ProductVariantAttributeCombination Create(
        int stockQuantity,
        decimal? price,
        string assignedProductImageIds,
        List<ProductVariantAttributeSelection> attributeSelections,
        bool isActive = true)
    {
        return new(
            ProductVariantAttributeCombinationId.CreateUnique(),
            price,
            stockQuantity,
            isActive,
            assignedProductImageIds,
            attributeSelections);
    }
}