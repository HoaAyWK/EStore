using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Entities;
using ErrorOr;

namespace EStore.Domain.ProductAggregate;

public sealed class Product : AggregateRoot<ProductId>, IAuditableEntity
{
    public const int MinNameLength = 2;

    public const int MaxNameLength = 200;

    public const decimal MinPrice = 0;

    private readonly List<ProductImage> _images = new();
    private readonly List<ProductAttribute> _productAttributes = new();
    private readonly List<ProductVariant> _productVariants = new();

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public decimal Price { get; private set; }

    public decimal? SpecialPrice { get; private set; }

    public DateTime? SpecialPriceStartDateTime { get; private set; }

    public DateTime? SpecialPriceEndDateTime { get; private set; }

    public bool Published { get; private set; }

    public CategoryId CategoryId { get; private set; } = null!;

    public BrandId BrandId { get; private set; } = null!;

    public AverageRating AverageRating { get; private set; } = null!;

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public IReadOnlyList<ProductImage> Images => _images.AsReadOnly();

    public IReadOnlyList<ProductAttribute> ProductAttributes => _productAttributes.AsReadOnly();

    public IReadOnlyList<ProductVariant> ProductVariants => _productVariants.AsReadOnly();

    private Product()
    {
    }

    private Product(
        ProductId productId,
        string name,
        string description,
        bool published,
        decimal price,
        BrandId brandId,
        CategoryId categoryId,
        AverageRating averageRating)
        : base(productId)
    {
        Name = name;
        Description = description;
        Published = published;
        Price = price;
        AverageRating = averageRating;
        BrandId = brandId;
        CategoryId = categoryId;
    }

    public static ErrorOr<Product> Create(
        string name,
        string description,
        bool published,
        BrandId brandId,
        CategoryId categoryId)
    {
        var errors = ValidateName(name);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Product(
            ProductId.CreateUnique(),
            name,
            description,
            published,
            0,
            brandId,
            categoryId,
            AverageRating.Create());
    }

    public void AddProductImage(ProductImage productImage)
    {
        _images.Add(productImage);
    }

    public void UpdatePublished(bool published)
    {
        Published = published;
    }

    public ErrorOr<Updated> UpdateDetails(
        string name,
        string description,
        decimal price)
    {
        var errors = ValidateName(name);
        errors.AddRange(ValidatePrice(price));

        if (errors.Count > 0)
        {
            return errors;
        }

        Name = name;
        Description = description;
        Price = price;

        return Result.Updated;
    }

    public ErrorOr<Updated> UpdateSpecialPrice(
        decimal specialPrice,
        DateTime startDate,
        DateTime endDate)
    {
        var errors = ValidatePrice(specialPrice);

        if (startDate < DateTime.UtcNow)
        {
            errors.Add(Errors.Product.InvalidSpecialPriceStartDate);
        }

        if (endDate <= startDate)
        {
            errors.Add(Errors.Product.InvalidSpecialPriceEndDate);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        SpecialPrice = specialPrice;
        SpecialPriceStartDateTime = startDate;
        SpecialPriceEndDateTime = endDate;

        return Result.Updated;
    }

    public void AddProductAttribute(ProductAttribute productAttribute)
    {
        _productAttributes.Add(productAttribute);
    }

    public void AddProductVariant(ProductVariant productVrProductVariant)
    {
        _productVariants.Add(productVrProductVariant);
    }

    public void UpdateCategory(CategoryId categoryId)
    {
        CategoryId = categoryId;
    }

    public void UpdateBrand(BrandId brandId)
    {
        BrandId = brandId;
    }

    private static List<Error> ValidateName(string name)
    {
        List<Error> errors = new();

        if (name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Product.InvalidNameLength);
        }

        return errors;
    }

    private List<Error> ValidatePrice(decimal price)
    {
        List<Error> errors = new();

        if (price < MinPrice)
        {
            errors.Add(Errors.Product.InvalidPrice);
        }

        return errors;
    }
}
