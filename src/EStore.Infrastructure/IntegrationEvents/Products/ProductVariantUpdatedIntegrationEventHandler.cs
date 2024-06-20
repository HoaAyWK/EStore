using System.Dynamic;
using Algolia.Search.Clients;
using Algolia.Search.Models.Settings;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Products.Events;
using EStore.Contracts.Searching;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.Common.Utilities;
using EStore.Domain.DiscountAggregate;
using EStore.Domain.DiscountAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Infrastructure.Services.AlgoliaSearch.Interfaces;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductVariantUpdatedIntegrationEventHandler
    : INotificationHandler<ProductVariantUpdatedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ISearchClient _searchClient;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IPriceCalculationService _priceCalculationService;
    private readonly IHierarchicalCategoryService _hierarchicalCategoryService;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;
    private readonly IAlgoliaIndexSettingsService _algoliaIndexSettingsService;
    private readonly ILogger<ProductVariantUpdatedIntegrationEventHandler> _logger;

    public ProductVariantUpdatedIntegrationEventHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        IDiscountRepository discountRepository,
        IPriceCalculationService priceCalculationService,
        IHierarchicalCategoryService hierarchicalCategoryService,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> algoliaSearchOptions,
        IAlgoliaIndexSettingsService algoliaIndexSettingsService,
        ILogger<ProductVariantUpdatedIntegrationEventHandler> logger)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _discountRepository = discountRepository;
        _priceCalculationService = priceCalculationService;
        _hierarchicalCategoryService = hierarchicalCategoryService;
        _searchClient = searchClient;
        _algoliaSearchOptions = algoliaSearchOptions.Value;
        _algoliaIndexSettingsService = algoliaIndexSettingsService;
        _logger = logger;
    }

    public async Task Handle(
        ProductVariantUpdatedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            return;
        }

        var productVariant = product.ProductVariants.SingleOrDefault(
            variant => variant.Id == notification.ProductVariantId);

        if (productVariant is null)
        {
            return;
        }

        if (product.Images.Count is 0)
        {
            return;
        }

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        if (!productVariant.IsActive)
        {
            try
            {
                var searchModel = await index.GetObjectAsync<ProductSearchModel>(
                    productVariant.Id.Value.ToString());

                if (searchModel is not null)
                {
                    await index.DeleteObjectAsync(productVariant.Id.Value.ToString());
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to delete product variant from search index");
            }

            return;
        }

        var mainImage = product.Images.Where(image => image.IsMain)
            .First();

        var orderedImages = product.Images
            .Where(image => image.Id != mainImage.Id)
            .OrderBy(image => image.DisplayOrder)
            .ToList();

        var assignedImageIds = productVariant.AssignedProductImageIds.ToLower().Split(' ');

        if (!assignedImageIds.Contains(mainImage.Id.Value.ToString()!.ToLower()))
        {
            // If the main image is not assigned to the product variant, assign the first image
            foreach (var image in orderedImages)
            {
                if (assignedImageIds.Contains(image.Id.Value.ToString()!.ToLower()))
                {
                    mainImage = image;
                    break;
                }
            }
        }
        
        ProductSearchModel? productSearchModel = null;
        
        try
        {
            productSearchModel = await index.GetObjectAsync<ProductSearchModel>(
                productVariant.Id.Value.ToString());
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to get product variant from search index");
        }

        var price = _priceCalculationService.CalculatePrice(product, productVariant);
        var finalPrice = price;
        Discount? discount = null;

        if (productSearchModel is null)
        {
            var category = await _categoryRepository.GetWithParentsByIdAsync(product.CategoryId);
            var brand = await _brandRepository.GetByIdAsync(product.BrandId);
            
            var hierarchyCategories = new ExpandoObject();

            if (product.DiscountId is not null)
            {
                discount = await _discountRepository.GetByIdAsync(product.DiscountId);
            }

            var attributeSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
                .Create(productVariant.RawAttributeSelection);
        
            if (category is not null)
            {
                hierarchyCategories = _hierarchicalCategoryService.GetHierarchy(category);
            }

            var searchIndexSettings = await _algoliaIndexSettingsService.GetIndexSettingsAsync();
            var attributesForFaceting = searchIndexSettings?.AttributesForFaceting.ToHashSet()
                ?? new HashSet<string>();

            finalPrice = _priceCalculationService.CalculatePrice(product, productVariant);

            if (discount is not null)
            {
                finalPrice = _priceCalculationService.ApplyDiscount(finalPrice, discount);
            }

            productSearchModel = new ProductSearchModel
            {
                ObjectID = notification.ProductVariantId.Value.ToString(),
                ProductVariantId = notification.ProductVariantId.Value,
                ProductId = notification.ProductId.Value,
                Name = product.Name,
                Description = product.Description,
                Price = price,
                FinalPrice = finalPrice,
                AverageRating = product.AverageRating.Value,
                DisplayOrder = product.DisplayOrder,
                CreatedDateTime = product.CreatedDateTime,
                UpdatedDateTime = product.UpdatedDateTime,
                HasVariant = product.HasVariant,
                Brand = brand?.Name,
                Image = mainImage.ImageUrl,
                IsActive = productVariant.IsActive,
                CategorySlug = category?.Slug
            };
        }
        else
        {
            productSearchModel.Image = mainImage.ImageUrl;
            productSearchModel.Price = _priceCalculationService.CalculatePrice(product, productVariant);
            productSearchModel.IsActive = productVariant.IsActive;

            if (discount is not null)
            {
                productSearchModel.FinalPrice = _priceCalculationService.ApplyDiscount(productSearchModel.Price, discount);
            }
            else
            {
                productSearchModel.FinalPrice = productSearchModel.Price;
            }
        }


        await index.SaveObjectAsync(productSearchModel);
    }
}
