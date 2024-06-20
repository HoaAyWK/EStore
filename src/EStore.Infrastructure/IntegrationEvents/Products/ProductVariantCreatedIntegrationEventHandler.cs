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

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductVariantCreatedIntegrationEventHandler
    : INotificationHandler<ProductVariantCreatedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IPriceCalculationService _priceCalculationService;
    private readonly IHierarchicalCategoryService _hierarchicalCategoryService;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;
    private readonly IAlgoliaIndexSettingsService _algoliaIndexSettingsService;

    public ProductVariantCreatedIntegrationEventHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        IDiscountRepository discountRepository,
        IPriceCalculationService priceCalculationService,
        IHierarchicalCategoryService hierarchicalCategoryService,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options,
        IAlgoliaIndexSettingsService algoliaIndexSettingsService)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _discountRepository = discountRepository;
        _priceCalculationService = priceCalculationService;
        _hierarchicalCategoryService = hierarchicalCategoryService;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
        _algoliaIndexSettingsService = algoliaIndexSettingsService;
    }

    public async Task Handle(
        ProductVariantCreatedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            // TODO: log error

            return;
        }

        if (!product.Published)
        {
            return;
        }

        var productVariant = product.ProductVariants.FirstOrDefault(
            v => v.Id == notification.ProductVariantId);

        if (productVariant is null)
        {
            // TODO: log error
            return;
        }

        if (!productVariant.IsActive)
        {
            return;
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

        var searchIndexSettings = await _algoliaIndexSettingsService.GetIndexSettingsAsync();
        var attributesForFaceting = searchIndexSettings?.AttributesForFaceting.ToHashSet()
            ?? new HashSet<string>();

        var mainImage = product.Images
            .Where(image => image.IsMain)
            .First();

        var orderedImages = product.Images
            .Where(image => image.Id != mainImage.Id)
            .OrderBy(image => image.DisplayOrder)
            .ToList();

        var assignedImageIds = productVariant.AssignedProductImageIds
            .ToLower()
            .Split(' ')
            .ToList();

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

        var productSearchModel = new ProductSearchModel
        {
            ObjectID = notification.ProductVariantId.Value.ToString(),
            ProductVariantId = notification.ProductVariantId.Value,
            ProductId = notification.ProductId.Value,
            Name = product.Name,
            Description = product.Description,
            Price = _priceCalculationService.CalculatePrice(product, productVariant),
            AverageRating = product.AverageRating.Value,
            DisplayOrder = product.DisplayOrder,
            CreatedDateTime = product.CreatedDateTime,
            UpdatedDateTime = product.UpdatedDateTime,
            HasVariant = product.HasVariant,
            IsActive = productVariant.IsActive,
            Brand = brand?.Name,
            Image =  mainImage?.ImageUrl,
            CategorySlug = category?.Slug
        };

        productSearchModel.FinalPrice = productSearchModel.Price;

        var productAttributes = new Dictionary<string, string>();

        // Add combined attributes
        foreach (var selection in attributeSelection.AttributesMap)
        {
            var attribute = product.ProductAttributes.FirstOrDefault(
                x => x.Id == selection.Key);

            if (attribute is null)
            {
                return;
            }

            var attributeValue = attribute.ProductAttributeValues.FirstOrDefault(
                x => x.Id == selection.Value.First());

            if (attributeValue is null)
            {
                return;
            }

            productAttributes.Add(attribute.Name, attributeValue.Name);

            var attributeFacingName = $"attributes.{attribute.Name}";

            if (!attributesForFaceting.Contains(attributeFacingName))
            {
                attributesForFaceting.Add(attributeFacingName);
            }
        }

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

            var attributeFacingName = $"attributes.{attribute.Name}";

            if (!attributesForFaceting.Contains(attributeFacingName))
            {
                attributesForFaceting.Add(attributeFacingName);
            }
        }

        if (discount is not null)
        {
            productSearchModel.Discount = new ProductSearchDiscount
            {
                DiscountName = discount.Name,
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
        await index.SetSettingsAsync(new IndexSettings
        {
            AttributesForFaceting = attributesForFaceting.ToList()
        });
    }
}
