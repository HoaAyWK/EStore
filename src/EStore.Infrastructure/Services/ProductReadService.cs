using EStore.Application.Products.Dtos;
using EStore.Application.Products.Services;
using EStore.Contracts.Common;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Services;

internal sealed class ProductReadService : IProductReadService
{
    private readonly EStoreDbContext _dbContext;

    public ProductReadService(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProductDto?> GetByIdAsync(ProductId id)
    {
        var productDto = await _dbContext.Products.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id.Value,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                SpecialPrice = p.SpecialPrice,
                SpecialPriceStartDate = p.SpecialPriceEndDateTime,
                SpecialPriceEndDate = p.SpecialPriceEndDateTime,
                Published = p.Published,
                StockQuantity = p.StockQuantity,
                DisplayOrder = p.DisplayOrder,
                AverageRating = new AverageRatingDto
                {
                    Value = p.AverageRating.Value,
                    NumRatings = p.AverageRating.NumRatings
                },
                Brand = _dbContext.Brands.AsNoTracking()
                    .Where(brand => brand.Id == p.BrandId)
                    .Select(brand => new BrandDto
                    {
                        Id = brand.Id.Value,
                        Name = brand.Name
                    })
                    .FirstOrDefault(),
                Category = _dbContext.Categories.AsNoTracking()
                    .Where(category => category.Id == p.CategoryId)
                    .Select(category => new CategoryDto
                    {
                        Id = category.Id.Value,
                        Name = category.Name
                    })
                    .FirstOrDefault(),
                Discount = _dbContext.Discounts.AsNoTracking()
                    .Where(discount => discount.Id == p.DiscountId!)
                    .Select(discount => new DiscountDto
                    {
                        Id = discount.Id.Value,
                        Name = discount.Name,
                        UsePercentage = discount.UsePercentage,
                        Percentage = discount.DiscountPercentage,
                        Amount = discount.DiscountAmount,
                        StartDate = discount.StartDateTime,
                        EndDate = discount.EndDateTime
                    })
                    .FirstOrDefault(),
                Images = p.Images.Select(image => new ProductImageDto
                {
                    Id = image.Id.Value,
                    ImageUrl = image.ImageUrl,
                    IsMain = image.IsMain,
                    DisplayOrder = image.DisplayOrder
                }),
                Attributes = p.ProductAttributes.Select(attribute =>
                        new ProductAttributeDto
                        {
                            Id = attribute.Id.Value,
                            Name = attribute.Name,
                            AttributeValues = attribute.ProductAttributeValues.Select(attributeValue =>
                            new ProductAttributeValueDto
                            {
                                Id = attributeValue.Id.Value,
                                Name = attributeValue.Name,
                                Alias = attributeValue.Alias,
                                PriceAdjustment = attributeValue.PriceAdjustment,
                                RawCombinedAttributes = attributeValue.RawCombinedAttributes
                            })
                        }),
                Variants = p.ProductVariants.Select(variant => new ProductVariantDto
                    {
                        Id = variant.Id.Value,
                        StockQuantity = variant.StockQuantity,
                        Price = variant.Price,
                        IsActive = variant.IsActive,
                        AssignedProductImageIds = variant.AssignedProductImageIds,
                        RawAttributeSelection = variant.RawAttributeSelection
                    })
            })
            .FirstOrDefaultAsync();

        return productDto;
    }

    public async Task<PagedList<ProductDto>> GetProductListPagedAsync(
        string? searchTerm,
        int page,
        int pageSize)
    {
        var productDtos = await _dbContext.Products.AsNoTracking()
            .Where(p => string.IsNullOrWhiteSpace(searchTerm) || p.Name.Contains(searchTerm))
            .OrderBy(p => p.DisplayOrder)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto
            {
                Id = p.Id.Value,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                SpecialPrice = p.SpecialPrice,
                SpecialPriceStartDate = p.SpecialPriceEndDateTime,
                SpecialPriceEndDate = p.SpecialPriceEndDateTime,
                Published = p.Published,
                StockQuantity = p.StockQuantity,
                DisplayOrder = p.DisplayOrder,
                AverageRating = new AverageRatingDto
                {
                    Value = p.AverageRating.Value,
                    NumRatings = p.AverageRating.NumRatings
                },
                Brand = _dbContext.Brands.AsNoTracking()
                    .Where(brand => brand.Id == p.BrandId)
                    .Select(brand => new BrandDto
                    {
                        Id = brand.Id.Value,
                        Name = brand.Name
                    })
                    .FirstOrDefault(),
                Category = _dbContext.Categories.AsNoTracking()
                    .Where(category => category.Id == p.CategoryId)
                    .Select(category => new CategoryDto
                    {
                        Id = category.Id.Value,
                        Name = category.Name
                    })
                    .FirstOrDefault(),
                Images = p.Images.Select(image => new ProductImageDto
                {
                    Id = image.Id.Value,
                    ImageUrl = image.ImageUrl,
                    IsMain = image.IsMain,
                    DisplayOrder = image.DisplayOrder
                }),
                Attributes = p.ProductAttributes.Select(attribute =>
                        new ProductAttributeDto
                        {
                            Id = attribute.Id.Value,
                            Name = attribute.Name,
                            AttributeValues = attribute.ProductAttributeValues.Select(attributeValue =>
                            new ProductAttributeValueDto
                            {
                                Id = attributeValue.Id.Value,
                                Name = attributeValue.Name,
                                Alias = attributeValue.Alias,
                                PriceAdjustment = attributeValue.PriceAdjustment,
                                RawCombinedAttributes = attributeValue.RawCombinedAttributes
                            })
                        }),
                Variants = p.ProductVariants.Select(variant => new ProductVariantDto
                    {
                        Id = variant.Id.Value,
                        StockQuantity = variant.StockQuantity,
                        Price = variant.Price,
                        IsActive = variant.IsActive,
                        AssignedProductImageIds = variant.AssignedProductImageIds,
                        RawAttributeSelection = variant.RawAttributeSelection
                    })
            })
            .ToListAsync();
            
        int totalCount = await CountTotalItems(searchTerm);

        return new PagedList<ProductDto>(productDtos, page, pageSize, totalCount);
    }

    private async Task<int> CountTotalItems(string? searchTerm)
    {
        var productQuery = _dbContext.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            productQuery.Where(p => p.Name.Contains(searchTerm));
        }

        var totalCount = await productQuery.CountAsync();

        return totalCount;
    }
}
