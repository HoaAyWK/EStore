using EStore.Domain.Common.Models;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using EStore.Domain.Catalog.CategoryAggregate;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using EStore.Domain.Catalog.BrandAggregate;
using EStore.Domain.Catalog.BrandAggregate.ValueObjects;

namespace EStore.Domain.Catalog.ProductAggregate;

public sealed class Product : AggregateRoot<ProductId>
{
    private readonly List<ProductImage> _images = new();
    private readonly List<ProductVariantAttribute> _productVariantAttributes = new();
    private readonly List<ProductVariantAttributeCombination> _productVariantAttributeCombinations = new();

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public decimal Price { get; private set; }

    public decimal? SpecialPrice { get; private set; }

    public DateTime? SpecialPriceStartDateTime { get; private set; }

    public DateTime? SpecialPriceEndDateTime { get; private set; }

    public bool Published { get; private set; }

    public AverageRating AverageRating { get; private set; } = null!;

    public BrandId BrandId { get; private set; } = null!;
    
    public Brand? Brand { get; private set; }

    public CategoryId CategoryId { get; private set; } = null!;

    public Category? Category { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public IReadOnlyList<ProductImage> Images => _images.AsReadOnly();

    public IReadOnlyList<ProductVariantAttribute> ProductVariantAttributes
        => _productVariantAttributes.AsReadOnly();

    public IReadOnlyList<ProductVariantAttributeCombination> ProductVariantAttributeCombinations
        => _productVariantAttributeCombinations.AsReadOnly();

    private Product()
    {
    }

    private Product(
        ProductId productId,
        string name,
        string description,
        bool published,
        AverageRating averageRating,
        BrandId brandId,
        CategoryId categoryId,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(productId)
    {
        Name = name;
        Description = description;
        Published = published;
        AverageRating = averageRating;
        BrandId = brandId;
        CategoryId = categoryId;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public static Product Create(
        string name,
        string description,
        bool published,
        BrandId brandId,
        CategoryId categoryId)
    {
        return new Product(
            ProductId.CreateUnique(),
            name,
            description,
            published,
            AverageRating.Create(),
            brandId,
            categoryId,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }

    public void AddProductImage(ProductImage productImage)
    {
        _images.Add(productImage);
    }

    public void UpdatePublished(bool published)
    {
        Published = published;
    }

    public void UpdateDetails(
        string name,
        string description,
        decimal price)
    {
        Name = name;
        Description = description;
        Price = price;
    }


    public void UpdateBrand(BrandId brandId)
    {
        BrandId = brandId;
    }

    public void UpdateCategory(CategoryId categoryId)
    {
        CategoryId = categoryId;
    }

    public void UpdateSpecialPrice(
        decimal specialPrice,
        DateTime startDate,
        DateTime endDate)
    {
        SpecialPrice = specialPrice;
        SpecialPriceStartDateTime = startDate;
        SpecialPriceEndDateTime = endDate;
    }
}
