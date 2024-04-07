using EStore.Domain.Common.Models;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Entities;
using ErrorOr;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.DiscountAggregate.ValueObjects;
using System.Text;

namespace EStore.Domain.ProductAggregate;

public sealed class Product : AggregateRoot<ProductId>, IAuditableEntity, ISoftDeletableEntity
{
    public const int MinNameLength = 2;

    public const int MaxNameLength = 200;

    public const decimal MinPrice = 0;

    public const int MinStockQuantity = 0;

    private readonly List<ProductImage> _images = new();
    private readonly List<ProductAttribute> _productAttributes = new();
    private readonly List<ProductVariant> _productVariants = new();
    private readonly List<ProductReview> _productReviews = new();

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public decimal Price { get; private set; }

    public int DisplayOrder { get; private set; }

    public int StockQuantity { get; private set; }

    public bool Published { get; private set; }

    public CategoryId CategoryId { get; private set; } = null!;

    public BrandId BrandId { get; private set; } = null!;

    public AverageRating AverageRating { get; private set; } = null!;

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public bool HasVariant { get; private set; }

    public DiscountId? DiscountId { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    public bool Deleted { get; private set; }
    
    public IReadOnlyList<ProductImage> Images => _images.AsReadOnly();

    public IReadOnlyList<ProductAttribute> ProductAttributes => _productAttributes.AsReadOnly();

    public IReadOnlyList<ProductVariant> ProductVariants => _productVariants.AsReadOnly();

    public IReadOnlyList<ProductReview> ProductReviews => _productReviews.AsReadOnly();

    private Product()
    {
    }

    private Product(
        ProductId productId,
        string name,
        string description,
        bool published,
        decimal price,
        int displayOrder,
        BrandId brandId,
        CategoryId categoryId,
        AverageRating averageRating,
        bool hasVariant)
        : base(productId)
    {
        Name = name;
        Description = description;
        Published = published;
        Price = price;
        AverageRating = averageRating;
        DisplayOrder = displayOrder;
        BrandId = brandId;
        CategoryId = categoryId;
        HasVariant = hasVariant;
    }

    public static ErrorOr<Product> Create(
        string name,
        string description,
        bool published,
        decimal price,
        int displayOrder,
        BrandId brandId,
        CategoryId categoryId,
        bool hasVariant)
    {
        var errors = ValidateName(name);

        errors.AddRange(ValidatePrice(price));

        if (errors.Count > 0)
        {
            return errors;
        }

        var product = new Product(
            ProductId.CreateUnique(),
            name,
            description,
            published,
            price,
            displayOrder,
            brandId,
            categoryId,
            AverageRating.Create(),
            hasVariant);

        product.RaiseDomainEvent(
            new ProductCreatedDomainEvent(product.Id));

        return product;
    }

    public void AddProductImage(ProductImage productImage)
    {
        _images.Add(productImage);
        RaiseDomainEvent(new ProductImageAddedDomainEvent(Id, productImage.Id));
    }

    public ErrorOr<Updated> UpdateDetails(
        string name,
        string description,
        decimal price,
        int displayOrder,
        bool published,
        bool hasVariant)
    {
        var errors = ValidateName(name);

        errors.AddRange(ValidatePrice(price));

        if (errors.Count > 0)
        {
            return errors;
        }

        if (!hasVariant && ProductVariants.Count > 0)
        {
            return Errors.Product.ProductHadVariants;
        }

        if (!hasVariant && ProductAttributes.Any(attribute => attribute.CanCombine))
        {
            return Errors.Product.ProductHadCombinableAttributes;
        }

        var previousHasVariant = HasVariant;

        Name = name;
        Description = description;
        Price = price;
        DisplayOrder = displayOrder;
        Published = published;

        // TODO: need to check if there is any cart item is this product
        HasVariant = hasVariant;

        RaiseDomainEvent(new ProductUpdatedDomainEvent(Id, previousHasVariant));

        return Result.Updated;
    }

    public void AddProductAttribute(ProductAttribute productAttribute)
    {
        _productAttributes.Add(productAttribute);
    }

    public ErrorOr<Updated> UpdateProductAttribute(
        ProductAttributeId id,
        string name,
        bool canCombine,
        int displayOrder)
    {
        if (!HasVariant)
        {
            return Errors.Product.NonVariantProductCannotHaveCombineAttributes;
        }

        var productAttribute = ProductAttributes.FirstOrDefault(x => x.Id == id);

        if (productAttribute is null)
        {
            return Errors.Product.ProductAttributeNotFound;
        }

        if (productAttribute.ProductAttributeValues.Count > 0)
        {
            return Errors.Product.ProductAttributeAlreadyHadValues;
        }

        productAttribute.Update(name, canCombine, displayOrder);

        return Result.Updated;
    }

    public ErrorOr<Created> AddProductAttributeValue(
        ProductAttributeId productAttributeId,
        string name,
        decimal? priceAdjustment,
        string? color,
        int displayOrder)
    {
        var productAttribute = ProductAttributes.FirstOrDefault(x =>
            x.Id == productAttributeId);

        if (productAttribute is null)
        {
            return Errors.Product.ProductAttributeNotFound;
        }

        if (!productAttribute.CanCombine &&
            productAttribute.ProductAttributeValues.Count > 0)
        {
            return Errors.Product.NonCombineProductAttributeCannotHaveMoreThanTwoValues;
        }

        var productAttributeValue = ProductAttributeValue.Create(
            name,
            priceAdjustment ?? 0,
            color,
            displayOrder);   

        productAttribute.AddAttributeValue(productAttributeValue);
        RaiseDomainEvent(new ProductAttributeValueAddedDomainEvent(
            Id,
            productAttributeId,
            productAttributeValue.Id));

        return Result.Created;
    }

    public ErrorOr<Updated> UpdateProductAttributeValue(
        ProductAttributeId attributeId,
        ProductAttributeValueId attributeValueId,
        string name,
        decimal? priceAdjustment,
        string? color,
        int displayOrder)
    {
        var productAttribute = ProductAttributes.FirstOrDefault(x => x.Id == attributeId);

        if (productAttribute is null)
        {
            return Errors.Product.ProductAttributeNotFound;
        }

        var productAttributeValue = productAttribute.ProductAttributeValues
            .FirstOrDefault(x => x.Id == attributeValueId);

        if (productAttributeValue is null)
        {
            return Errors.Product.ProductAttributeValueNotFound;
        }

        decimal attributeValueOldPrice = productAttributeValue.PriceAdjustment;
        decimal attributeValueNewPrice = priceAdjustment ?? 0;

        productAttributeValue.UpdateDetails(
            name,
            color,
            attributeValueNewPrice,
            displayOrder);

        RaiseDomainEvent(new ProductAttributeValueUpdatedDomainEvent(
            Id,
            attributeId,
            attributeValueId,
            attributeValueOldPrice,
            attributeValueNewPrice));
        
        return Result.Updated;
    }

    public void AddProductVariant(ProductVariant productVariant)
    {
        _productVariants.Add(productVariant);
        RaiseDomainEvent(new ProductVariantCreatedDomainEvent(Id, productVariant.Id));
    }

    public ErrorOr<Updated> UpdateProductVariant(
        ProductVariantId productVariantId,
        int stockQuantity,
        bool isActive,
        List<Guid>? imageIds = null)
    {
        var productVariant = ProductVariants.FirstOrDefault(v => v.Id == productVariantId);

        if (productVariant is null)
        {
            return Errors.Product.ProductVariantNotFound;
        }

        var updateDetailsResult = productVariant.UpdateDetails(
            stockQuantity,
            isActive);

        if (updateDetailsResult.IsError)
        {
            return updateDetailsResult.Errors;
        }

        var assignedImageIds = new StringBuilder();

        if (imageIds is not null)
        {
            for (int i = 0; i < imageIds.Count; i++)
            {
                var assignedId = imageIds.ElementAt(i);
                var productImageId = Images.FirstOrDefault(
                    image => image.Id == ProductImageId.Create(assignedId));

                if (productImageId is null)
                {
                    return Errors.Product.ProductImageNotFound;
                }

                if (i is 0)
                {
                    assignedImageIds.Append(assignedId.ToString().ToLower());
                }
                else
                {
                    assignedImageIds.Append($" {assignedId.ToString().ToLower()}");
                }
            }

            productVariant.UpdateAssignedImageIds(assignedImageIds.ToString());
        }

        RaiseDomainEvent(new ProductVariantUpdatedDomainEvent(Id, productVariantId));

        return Result.Updated;
    }

    public void UpdateCategory(CategoryId categoryId)
    {
        CategoryId = categoryId;
    }

    public void UpdateBrand(BrandId brandId)
    {
        BrandId = brandId;
    }

    public ErrorOr<Updated> UpdateStockQuantity(int stockQuantity)
    {
        if (stockQuantity < MinStockQuantity)
        {
            return Errors.Product.InvalidStockQuantity;
        }

        StockQuantity = stockQuantity;

        return Result.Updated;
    }

    public ErrorOr<Deleted> RemoveAttributeValue(
        ProductAttributeId attributeId,
        ProductAttributeValueId attributeValueId)
    {
        var attribute = _productAttributes.FirstOrDefault(
            x => x.Id == attributeId);

        if (attribute is null)
        {
            return Errors.Product.ProductAttributeNotFound;
        }

        var attributeValue = attribute.ProductAttributeValues.FirstOrDefault(
            x => x.Id == attributeValueId);

        if (attributeValue is null)
        {
            return Errors.Product.ProductAttributeValueNotFound;
        }

        if (HasVariant && _productVariants.Count > 0)
        {
            return Errors.Product.ProductAlreadyHadVariants;
        }

        attribute.RemoveAttributeValue(attributeValue);

        RaiseDomainEvent(new ProductAttributeValueRemovedDomainEvent(
            Id,
            attributeId,
            attributeValueId));

        return Result.Deleted;
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

    private static List<Error> ValidatePrice(decimal price)
    {
        List<Error> errors = new();

        if (price <= MinPrice)
        {
            errors.Add(Errors.Product.InvalidPrice);
        }

        return errors;
    }
}
