using ErrorOr;
using EStore.Application.Banners.Services;
using EStore.Contracts.Banners;
using EStore.Contracts.Common;
using EStore.Domain.BannerAggregate;
using EStore.Domain.BannerAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate;
using EStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EStore.Infrastructure.Services;

public class BannerReadService : IBannerReadService
{
    private readonly EStoreDbContext _dbContext;

    public BannerReadService(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<BannerResponse>> GetByIdAsync(BannerId id)
    { 
        var banner = await _dbContext.Banners.AsNoTracking()
            .Where(banner => banner.Id == id)
            .SingleOrDefaultAsync();

        if (banner is null)
        {
            return Errors.Banner.NotFound;
        }

        var product = await _dbContext.Products.AsNoTracking()
            .Where(product => product.Id == banner.ProductId)
            .SingleOrDefaultAsync();

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        return Map(banner, product);
    }

    public async Task<PagedList<BannerResponse>> GetListPagedAsync(
        int page,
        int pageSize,
        string? order,
        string? orderBy)
    {
        var query = _dbContext.Banners.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(order))
        {
            if (order == "asc")
            {
                query = orderBy switch
                {
                    "displayOrder" => query.OrderBy(banner => banner.DisplayOrder),
                    _ => query.OrderBy(banner => banner.CreatedDateTime)
                };
            }
            else if (order == "desc")
            {
                query = orderBy switch
                {
                    "displayOrder" => query.OrderByDescending(banner => banner.DisplayOrder),
                    _ => query.OrderByDescending(banner => banner.CreatedDateTime)
                };
            }
        }

        var bannersWithProductQuery = from banner in query
            join product in _dbContext.Products.AsNoTracking()
            on banner.ProductId equals product.Id
            select new
            {
                Banner = banner,
                Product = product
            };

        if (page is not 0 && pageSize is not 0)
        {
            bannersWithProductQuery = bannersWithProductQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        var bannersWithProduct = await bannersWithProductQuery.ToListAsync();
        var totalItems = await _dbContext.Banners.CountAsync();

        var bannerResponses = new List<BannerResponse>();

        foreach (var banner in bannersWithProduct)
        {
            if (banner.Banner is null || banner.Product is null)
            {
                continue;
            }

            bannerResponses.Add(Map(banner.Banner, banner.Product));
        }

        var totalPages = 0;

        if (page is not 0 || pageSize is not 0)
        {
            totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        }

        return new PagedList<BannerResponse>(
            bannerResponses,
            page,
            pageSize,
            totalItems,
            totalPages);
    }

    private BannerResponse Map(Banner banner, Product product)
    {
        var bannerResponse = new BannerResponse
        {
            Id = banner.Id.Value,
            Direction = banner.Direction.Name,
            DisplayOrder = banner.DisplayOrder,
            IsActive = banner.IsActive,
            CreatedDateTime = banner.CreatedDateTime
        };

        var mainImage = product.Images
                .Where(image => image.IsMain)
                .SingleOrDefault();

        var productResponse = new ProductResponse
        {
            Id = product.Id.Value,
            Name = product.Name,
            ShortDescription = product.ShortDescription,
            ImageUrl = mainImage?.ImageUrl
        };

        if (banner.ProductVariantId is not null)
        {
            var productVariant = product.ProductVariants
                .Where(variant => variant.Id == banner.ProductVariantId)
                .SingleOrDefault();

            if (productVariant is not null)
            {
                var imageIds = productVariant.AssignedProductImageIds.Split(' ');

                if (!imageIds.Contains(mainImage?.Id.ToString()))
                {
                    var variantFirstImageId = imageIds.FirstOrDefault();

                    if (variantFirstImageId is not null)
                    {
                        var image = product.Images
                            .Where(image => image.Id.Value.ToString() == variantFirstImageId)
                            .SingleOrDefault();

                        if (image is not null)
                        {
                            productResponse.ImageUrl = image.ImageUrl;
                        }
                    }
                }
                
                var attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    productVariant.RawAttributes)
                    ?? new();

                productResponse.ProductVariant = new ProductVariantResponse(
                    Id: productVariant.Id.Value,
                    Attributes: attributes);
            }
        }
    
        bannerResponse.Product = productResponse;

        return bannerResponse;
    }
}
