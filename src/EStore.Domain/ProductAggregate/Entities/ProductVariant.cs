using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Entities;

public sealed class ProductVariant : Entity<ProductVariantId>
{
    public const int MinStockQuantity = 0;

    public const decimal MinPrice = 0;

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
        decimal? price,
        int stockQuantity,
        bool isActive,
        string? rawAttributeSelection,
        string assignedProductImageIds)
        : base(id)
    {
        Price = price;
        StockQuantity = stockQuantity;
        IsActive = isActive;
        RawAttributeSelection = rawAttributeSelection;
        AssignedProductImageIds = assignedProductImageIds;
    }

    public static ErrorOr<ProductVariant> Create(
        int stockQuantity,
        decimal price,
        string assignedProductImageIds,
        string? rawAttributeSelection,
        bool isActive = true)
    {
        var errors = new List<Error>();

        var validatePriceResult = ValidatePrice(price);
        var validateStockQuantityResult = ValidateQuantity(stockQuantity);

        if (validateStockQuantityResult.IsError)
        {
            errors.Add(validateStockQuantityResult.FirstError);
        }

        if (validatePriceResult.IsError)
        {
            errors.Add(validatePriceResult.FirstError);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new ProductVariant(
            ProductVariantId.CreateUnique(),
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
        bool isActive)
    {

        var validateStockQuantityResult = ValidateQuantity(stockQuantity);

        if (validateStockQuantityResult.IsError)
        {
            return validateStockQuantityResult.Errors;
        }

        StockQuantity = stockQuantity;
        IsActive = isActive;

        return Result.Updated;
    }


    public ErrorOr<Updated> UpdateStockQuantity(int stockQuantity)
    {
        var validateStockQuantityResult = ValidateQuantity(stockQuantity);

        if (validateStockQuantityResult.IsError)
        {
            return validateStockQuantityResult.Errors;
        }

        StockQuantity = stockQuantity;

        return Result.Updated;
    }

    public ErrorOr<Updated> UpdatePrice(decimal? price)
    {
        var validatePriceResult = ValidatePrice(price ?? 0);

        if (validatePriceResult.IsError)
        {
            return validatePriceResult.Errors;
        }

        Price = price;

        return Result.Updated;
    }

    public void UpdateAssignedImageIds(string assignedImageIds)
    {
        AssignedProductImageIds = assignedImageIds;
    }

    private static ErrorOr<Success> ValidatePrice(decimal? price)
    {  
        if (price < MinPrice)
        {
            return Errors.Product.InvalidProductVariantPrice;
        }

        return Result.Success;
    }

    private static ErrorOr<Success> ValidateQuantity(int stockQuantity)
    {
        if (stockQuantity < MinStockQuantity)
        {
            return Errors.Product.InvalidProductVariantStockQuantity;
        }

        return Result.Success;
    }
}
