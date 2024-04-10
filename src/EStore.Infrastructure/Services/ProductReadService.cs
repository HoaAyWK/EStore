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
            .AsSplitQuery()
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id.Value,
                ObjectID = p.Id.Value.ToString(),
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Published = p.Published,
                StockQuantity = p.StockQuantity,
                DisplayOrder = p.DisplayOrder,
                CreatedDateTime = p.CreatedDateTime,
                UpdatedDateTime = p.UpdatedDateTime,
                HasVariant = p.HasVariant,
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
                Images = p.Images
                    .OrderBy(image => image.DisplayOrder)
                    .Select(image => new ProductImageDto
                    {
                        Id = image.Id.Value,
                        ImageUrl = image.ImageUrl,
                        IsMain = image.IsMain,
                        DisplayOrder = image.DisplayOrder
                    }),
                Attributes = p.ProductAttributes
                    .OrderBy(attribute => attribute.DisplayOrder)
                    .Select(attribute =>
                        new ProductAttributeDto
                        {
                            Id = attribute.Id.Value,
                            Name = attribute.Name,
                            CanCombine = attribute.CanCombine,
                            DisplayOrder = attribute.DisplayOrder,
                            Colorable = attribute.Colorable,
                            AttributeValues = attribute.ProductAttributeValues
                                .OrderBy(attributeValue => attributeValue.DisplayOrder)
                                .Select(attributeValue =>
                                    new ProductAttributeValueDto
                                    {
                                        Id = attributeValue.Id.Value,
                                        Name = attributeValue.Name,
                                        Color = attributeValue.Color,
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
                        RawAttributeSelection = variant.RawAttributeSelection,
                        RawAttributes = variant.RawAttributes
                    }),
                Reviews = p.ProductReviews.Select(review => new ProductReviewDto
                    {
                        Id = review.Id.Value,
                        Title = review.Title,
                        Content = review.Content,
                        Rating = review.Rating,
                        RawAttributeSelection = review.RawAttributes,
                        Owner = _dbContext.Customers.AsNoTracking()
                            .Where(customer => customer.Id == review.OwnerId)
                            .Select(customer => new ProductReviewOwnerDto
                            {
                                Id = customer.Id.Value,
                                FirstName = customer.FirstName,
                                LastName = customer.LastName,
                                Email = customer.Email,
                                AvatarUrl = customer.AvatarUrl
                            })
                            .FirstOrDefault(),
                        CreatedDateTime = review.CreatedDateTime,
                        UpdatedDateTime = review.UpdatedDateTime,
                        ReviewComments = review.ReviewComments.Select(comment => new ProductReviewCommentDto
                            {
                                Id = comment.Id.Value,
                                Content = comment.Content,
                                CreatedDateTime = comment.CreatedDateTime,
                                UpdatedDateTime = comment.UpdatedDateTime,
                                Owner = _dbContext.Customers.AsNoTracking()
                                    .Where(customer => customer.Id == comment.OwnerId)
                                    .Select(customer => new ProductReviewOwnerDto
                                    {
                                        Id = customer.Id.Value,
                                        FirstName = customer.FirstName,
                                        LastName = customer.LastName,
                                        Email = customer.Email,
                                        AvatarUrl = customer.AvatarUrl
                                    })
                                    .FirstOrDefault()
                            })
                    })
            })
            .FirstOrDefaultAsync();

        return productDto;
    }

    public async Task<PagedList<ProductDto>> GetProductListPagedAsync(
        string? searchTerm,
        int page,
        int pageSize,
        string? order,
        string? orderBy)
    {
        var productDtosQuery = _dbContext.Products.AsNoTracking()
            .Where(p => string.IsNullOrWhiteSpace(searchTerm) || p.Name.Contains(searchTerm))
            .Select(p => new ProductDto
            {
                Id = p.Id.Value,
                ObjectID = p.Id.Value.ToString(),
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Published = p.Published,
                StockQuantity = p.StockQuantity,
                DisplayOrder = p.DisplayOrder,
                CreatedDateTime = p.CreatedDateTime,
                UpdatedDateTime = p.UpdatedDateTime,
                HasVariant = p.HasVariant,
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
                Images = p.Images
                    .OrderBy(image => image.DisplayOrder)
                    .Select(image => new ProductImageDto
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
                            CanCombine = attribute.CanCombine,
                            Colorable = attribute.Colorable,
                            AttributeValues = attribute.ProductAttributeValues.Select(attributeValue =>
                            new ProductAttributeValueDto
                            {
                                Id = attributeValue.Id.Value,
                                Name = attributeValue.Name,
                                Color = attributeValue.Color,
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
                        RawAttributeSelection = variant.RawAttributeSelection,
                        RawAttributes = variant.RawAttributes
                    }),
                Reviews = p.ProductReviews.Select(review => new ProductReviewDto
                    {
                        Id = review.Id.Value,
                        Title = review.Title,
                        Content = review.Content,
                        Rating = review.Rating,
                        RawAttributeSelection = review.RawAttributes,
                        Owner = _dbContext.Customers.AsNoTracking()
                            .Where(customer => customer.Id == review.OwnerId)
                            .Select(customer => new ProductReviewOwnerDto
                            {
                                Id = customer.Id.Value,
                                FirstName = customer.FirstName,
                                LastName = customer.LastName,
                                Email = customer.Email,
                                AvatarUrl = customer.AvatarUrl
                            })
                            .FirstOrDefault(),
                        CreatedDateTime = review.CreatedDateTime,
                        UpdatedDateTime = review.UpdatedDateTime,
                        ReviewComments = review.ReviewComments.Select(comment => new ProductReviewCommentDto
                            {
                                Id = comment.Id.Value,
                                Content = comment.Content,
                                CreatedDateTime = comment.CreatedDateTime,
                                UpdatedDateTime = comment.UpdatedDateTime,
                                Owner = _dbContext.Customers.AsNoTracking()
                                    .Where(customer => customer.Id == comment.OwnerId)
                                    .Select(customer => new ProductReviewOwnerDto
                                    {
                                        Id = customer.Id.Value,
                                        FirstName = customer.FirstName,
                                        LastName = customer.LastName,
                                        Email = customer.Email,
                                        AvatarUrl = customer.AvatarUrl
                                    })
                                    .FirstOrDefault()
                            })
                    })
            });
        
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            if (!string.IsNullOrWhiteSpace(order) && order.ToLower() == "desc")
            {
                productDtosQuery = orderBy switch
                {
                    "name" => productDtosQuery.OrderByDescending(p => p.Name),
                    "price" => productDtosQuery.OrderByDescending(p => p.Price),
                    "stockQuantity" => productDtosQuery.OrderByDescending(p => p.StockQuantity),
                    "displayOrder" => productDtosQuery.OrderByDescending(p => p.DisplayOrder),
                    "updatedDateTime" => productDtosQuery.OrderByDescending(p => p.UpdatedDateTime),
                    _ => productDtosQuery.OrderByDescending(p => p.CreatedDateTime)
                };
            }
            else
            {
                productDtosQuery = orderBy switch
                {
                    "name" => productDtosQuery.OrderBy(p => p.Name),
                    "price" => productDtosQuery.OrderBy(p => p.Price),
                    "stockQuantity" => productDtosQuery.OrderBy(p => p.StockQuantity),
                    "displayOrder" => productDtosQuery.OrderBy(p => p.DisplayOrder),
                    "updatedDateTime" => productDtosQuery.OrderBy(p => p.UpdatedDateTime),
                    _ => productDtosQuery.OrderBy(p => p.CreatedDateTime),
                };
            }
        }

        productDtosQuery = productDtosQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var productDtos = await productDtosQuery.ToListAsync();   
        int totalCount = await CountTotalItems(searchTerm);
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedList<ProductDto>(productDtos, page, pageSize, totalCount, totalPages);
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
