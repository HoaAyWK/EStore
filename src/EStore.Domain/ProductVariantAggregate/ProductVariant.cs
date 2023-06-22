using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.ValueObjects;

namespace EStore.Domain.ProductVariantAggregate;

public sealed class ProductVariant : Entity<ProductVariantId>
{
    public const int MinStockQuantity = 0;

    public const decimal MinPrice = 0;

    public ProductId ProductId { get; private set; } = null!;

    public decimal? Price { get; private set; }

    public int StockQuantity { get; private set; }

    public bool IsActive { get; private set; }

    public string AssignedProductImageIds { get; private set; } = null!;

    public string? RawAttributeSelection { get; private set; }

    private ProductVariant()
    {
    }

    private ProductVariant(
        ProductVariantId id,
        ProductId productId,
        decimal? price,
        int stockQuantity,
        bool isActive,
        string? rawAttributeSelection,
        string assignedProductImageIds)
        : base(id)
    {
        ProductId = productId;
        Price = price;
        StockQuantity = stockQuantity;
        IsActive = isActive;
        RawAttributeSelection = rawAttributeSelection;
        AssignedProductImageIds = assignedProductImageIds;
    }

    public static ErrorOr<ProductVariant> Create(
        ProductId productId,
        int stockQuantity,
        decimal? price,
        string assignedProductImageIds,
        string? rawAttributeSelection,
        bool isActive = true)
    {
        var errors =  ValidateQuantityAndPrice(stockQuantity, price);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new ProductVariant(
            ProductVariantId.CreateUnique(),
            productId,
            price,
            stockQuantity,
            isActive,
            rawAttributeSelection,
            assignedProductImageIds);
    }

    public void UpdateRawAttributes(string? rawAttributes)
    {
        RawAttributeSelection = rawAttributes;
    }

    public ErrorOr<Updated> UpdateDetails(
        int stockQuantity,
        decimal? price,
        bool isActive)
    {
        var errors =  ValidateQuantityAndPrice(stockQuantity, price);

        if (errors.Count > 0)
        {
            return errors;
        }

        StockQuantity = stockQuantity;
        Price = price;
        IsActive = isActive;

        return Result.Updated;
    }

    public void UpdateAssignedImageIds(string assignedImageIds)
    {
        AssignedProductImageIds = assignedImageIds;
    }

    private static List<Error> ValidateQuantityAndPrice(int stockQuantity, decimal? price)
    {
        var errors = new List<Error>();

        if (stockQuantity < MinStockQuantity)
        {
            errors.Add(Errors.ProductVariant.InvalidStockQuantity);
        }

        if (price < MinPrice)
        {
            errors.Add(Errors.ProductVariant.InvalidPrice);
        }

        return errors;
    }

}