using System.Dynamic;
using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using ErrorOr;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Common.Searching;
using EStore.Contracts.Searching;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Utilities;
using EStore.Domain.DiscountAggregate;
using EStore.Domain.DiscountAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EStore.Infrastructure.Searching;

public class SearchProductsService : ISearchProductsService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IHierarchicalCategoryService _hierarchicalCategoryService;
    private readonly IPriceCalculationService _priceCalculationService;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public SearchProductsService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        IDiscountRepository discountRepository,
        IHierarchicalCategoryService hierarchicalCategoryService,
        IPriceCalculationService priceCalculationService,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _discountRepository = discountRepository;
        _hierarchicalCategoryService = hierarchicalCategoryService;
        _priceCalculationService = priceCalculationService;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
    }

    public async Task<ErrorOr<RebuildResult>> RebuildProductAsync(
        ProductId productId,
        ProductVariantId productVariantId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        if (!product.Published)
        {
            return Errors.Product.NotPublishedYet;
        }

        var productVariant = product.ProductVariants.FirstOrDefault(
            v => v.Id == productVariantId);

        if (productVariant is null)
        {
            return Errors.Product.ProductVariantNotFound;
        }

        var category = await _categoryRepository.GetWithParentsByIdAsync(product.CategoryId);
        var brand = await _brandRepository.GetByIdAsync(product.BrandId);
        Discount? discount = null;
        var hierarchyCategories = new ExpandoObject();

        if (product.DiscountId is not null)
        {
            discount = await _discountRepository.GetByIdAsync(product.DiscountId);
        }

        var attributeSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
            .Create(productVariant.RawAttributeSelection);
    
        var price = product.Price;

        if (category is not null)
        {
            hierarchyCategories = _hierarchicalCategoryService.GetHierarchy(category);
        }

        var productSearchModel = new ProductSearchModel
        {
            ObjectID = productVariantId.Value.ToString(),
            ProductVariantId = productVariantId.Value,
            ProductId = productId.Value,
            Name = product.Name,
            Description = product.Description,
            Price = _priceCalculationService.CalculatePrice(product, productVariant),
            AverageRating = productVariant.AverageRating.Value,
            NumRatings = productVariant.AverageRating.NumRatings,
            DisplayOrder = product.DisplayOrder,
            CreatedDateTime = product.CreatedDateTime,
            UpdatedDateTime = product.UpdatedDateTime,
            HasVariant = product.HasVariant,
            IsActive = productVariant.IsActive,
            Brand = brand?.Name,
            Image = product.Images
                .Where(image => image.IsMain)
                .Select(image => image.ImageUrl)
                .FirstOrDefault() ?? string.Empty
        };

        var productAttributes =
            JsonConvert.DeserializeObject<Dictionary<string, string>>(
                productVariant.RawAttributes)
                ?? new();

        // Add non-combined attributes
        foreach (var attribute in product.ProductAttributes)
        {
            if (attribute.CanCombine)
            {
                continue;
            }

            var attributeValue = attribute.ProductAttributeValues
                .ToList()
                .FirstOrDefault();

            if (attributeValue is not null)
            {
                productAttributes.Add(attribute.Name, attributeValue.Name);
            }
        }

        if (discount is not null)
        {
            productSearchModel.Discount = new ProductSearchDiscount
            {
                UsePercentage = discount.UsePercentage,
                DiscountPercentage = discount.DiscountPercentage,
                DiscountAmount = discount.DiscountAmount,
                StartDateTime = discount.StartDateTime,
                EndDateTime = discount.EndDateTime
            };

            productSearchModel.FinalPrice = _priceCalculationService.ApplyDiscount(
                productSearchModel.Price,
                discount);
        }

        productSearchModel.Attributes = productAttributes;
        productSearchModel.Categories = hierarchyCategories;

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        await index.SaveObjectAsync(productSearchModel);

        return new RebuildResult(productId.Value, productVariantId.Value);
    }

    public async Task<SearchProductListPagedResponse> SearchProductsAsync(
        string? searchQuery,
        int page,
        int pageSize)
    {
        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        var query = new Query(searchQuery)
        {
            Page = page,
            HitsPerPage = pageSize
        };

        var result = await index.SearchAsync<ProductSearchModel>(query);

        return new SearchProductListPagedResponse
        {
            Hits = result.Hits,
            Page = result.Page,
            PageSize = result.HitsPerPage,
            TotalHits = result.NbHits,
            TotalPages = result.NbPages
        };
    }
}
