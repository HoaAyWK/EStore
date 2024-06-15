using System.Dynamic;
using Algolia.Search.Clients;
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
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductUpdatedIntegrationEventHandler : INotificationHandler<ProductUpdatedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IPriceCalculationService _priceCalculationService;
    private readonly IHierarchicalCategoryService _hierarchicalCategoryService;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductUpdatedIntegrationEventHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        IDiscountRepository discountRepository,
        IPriceCalculationService priceCalculationService,
        IHierarchicalCategoryService hierarchicalCategoryService,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _discountRepository = discountRepository;
        _priceCalculationService = priceCalculationService;
        _hierarchicalCategoryService = hierarchicalCategoryService;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
    }

    public async Task Handle(
        ProductUpdatedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            // TODO: log

            return;
        }

        if (!product.Published)
        {
            return;
        }

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);
        var brand = await _brandRepository.GetByIdAsync(product.BrandId);
        var category = await _categoryRepository.GetWithParentsByIdAsync(
            product.CategoryId);

        var hierarchyCategories = new ExpandoObject();

        if (category is not null)
        {
            hierarchyCategories = _hierarchicalCategoryService.GetHierarchy(category);
        }

        Discount? discount = null;

        if (product.DiscountId is not null)
        {
            discount = await _discountRepository.GetByIdAsync(product.DiscountId);
        }

        if (notification.PreviousHasVariant != product.HasVariant)
        {
            if (product.HasVariant)
            {
                await index.DeleteObjectAsync(product.Id.Value.ToString(), ct: cancellationToken);

                return;
            }

            var singleProduct = new ProductSearchModel
            {
                ObjectID = product.Id.Value.ToString(),
                ProductId = product.Id.Value,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Categories = hierarchyCategories,
                Brand = brand?.Name,
                AverageRating = product.AverageRating.Value,
                DisplayOrder = product.DisplayOrder,
                CreatedDateTime = product.CreatedDateTime,
                UpdatedDateTime = product.UpdatedDateTime,
                Image = product.Images
                    .Where(image => image.IsMain)
                    .Select(image => image.ImageUrl)
                    .FirstOrDefault() ?? string.Empty
            };

            if (discount is not null)
            {
                singleProduct.Discount = new ProductSearchDiscount
                {
                    DiscountName = discount.Name,
                    UsePercentage = discount.UsePercentage,
                    DiscountPercentage = discount.DiscountPercentage,
                    DiscountAmount = discount.DiscountAmount,
                    StartDateTime = discount.StartDateTime,
                    EndDateTime = discount.EndDateTime
                };

                singleProduct.FinalPrice = _priceCalculationService.ApplyDiscount(
                    singleProduct.Price,
                    discount);
            }

            await index.SaveObjectAsync(singleProduct, ct: cancellationToken);

            return;
        }

        if (product.HasVariant)
        {
            var productVariantIds = product.ProductVariants
                .Select(x => x.Id.Value.ToString())
                .ToArray();

            var productSearchModels = await index.GetObjectsAsync<ProductSearchModel>(
                productVariantIds);

            foreach (var model in productSearchModels)
            {
                var variant = product.ProductVariants.Where(variant =>
                    variant.Id == ProductVariantId.Create(new Guid(model.ObjectID)))
                    .SingleOrDefault();

                model.Name = product.Name;
                model.Description = product.Description;
                model.ShortDescription = product.ShortDescription;
                model.Categories = hierarchyCategories;
                model.Brand = brand?.Name;
                model.DisplayOrder = product.DisplayOrder;
                model.UpdatedDateTime = product.UpdatedDateTime;

                if (variant is not null)
                {
                    model.Price = _priceCalculationService.CalculatePrice(product, variant);
                    
                    if (discount is not null)
                    {
                        model.Discount = new ProductSearchDiscount
                        {
                            DiscountName = discount.Name,
                            UsePercentage = discount.UsePercentage,
                            DiscountPercentage = discount.DiscountPercentage,
                            DiscountAmount = discount.DiscountAmount,
                            StartDateTime = discount.StartDateTime,
                            EndDateTime = discount.EndDateTime
                        };

                        model.FinalPrice = _priceCalculationService.ApplyDiscount(model.Price, discount);
                    }
                }

                await index.SaveObjectsAsync(productSearchModels);

                return;
            }
        }

        var productSearchModel = await index.GetObjectAsync<ProductSearchModel>(
            product.Id.Value.ToString());

        productSearchModel.Name = product.Name;
        productSearchModel.Description = product.Description;
        productSearchModel.Price = product.Price;
        productSearchModel.Categories = hierarchyCategories;
        productSearchModel.Brand = brand?.Name;
        productSearchModel.DisplayOrder = product.DisplayOrder;
        productSearchModel.UpdatedDateTime = product.UpdatedDateTime;

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

        await index.SaveObjectAsync(productSearchModel);
    }
}
