using Algolia.Search.Clients;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Products.Events;
using EStore.Contracts.Searching;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.DiscountAggregate;
using EStore.Domain.DiscountAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
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
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductUpdatedIntegrationEventHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        IDiscountRepository discountRepository,
        IPriceCalculationService priceCalculationService,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _discountRepository = discountRepository;
        _priceCalculationService = priceCalculationService;
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

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);
        var brand = await _brandRepository.GetByIdAsync(product.BrandId);
        var category = await _categoryRepository.GetWithParentsByIdAsync(
            product.CategoryId);

        var hierarchyCategories = new LinkedList<string>();

        if (category is not null)
        {
            var current = category;

            while (current is not null)
            {
                hierarchyCategories.AddFirst(current.Name);
                current = current.Parent;
            }
        }

        Discount? discount = null;

        if (product.DiscountId is not null)
        {
            discount = await _discountRepository.GetByIdAsync(product.DiscountId);
        }

        if (product.HasVariant)
        {
            var productSearchModels = new List<ProductSearchModel>();

            foreach (var variant in product.ProductVariants)
            {
                var model = new ProductSearchModel
                {
                    ObjectID = variant.Id.Value.ToString(),
                    ProductId = product.Id.Value,
                    ProductVariantId = variant.Id.Value,
                    Name = product.Name,
                    Description = product.Description,
                    Categories = hierarchyCategories.ToList(),
                    Brand = brand?.Name,
                    AverageRating = product.AverageRating.Value,
                    DisplayOrder = product.DisplayOrder,
                    CreatedDateTime = product.CreatedDateTime,
                    UpdatedDateTime = product.UpdatedDateTime,
                    Image = product.Images
                        .Where(image => image.IsMain)
                        .Select(image => image.ImageUrl)
                        .FirstOrDefault() ?? string.Empty,
                    Price = _priceCalculationService.CalculatePrice(product, variant)
                };
                
                if (discount is not null)
                {
                    model.Discount = new ProductSearchDiscount
                    {
                        UsePercentage = discount.UsePercentage,
                        DiscountPercentage = discount.DiscountPercentage,
                        DiscountAmount = discount.DiscountAmount,
                        StartDateTime = discount.StartDateTime,
                        EndDateTime = discount.EndDateTime
                    };

                    model.FinalPrice = _priceCalculationService.ApplyDiscount(
                        model.Price,
                        discount);
                }

                productSearchModels.Add(model);
            }

            await index.PartialUpdateObjectsAsync(productSearchModels);

            return;
        }

        var productSearchModel = new ProductSearchModel
        {
            ObjectID = product.Id.Value.ToString(),
            ProductId = product.Id.Value,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Categories = hierarchyCategories.ToList(),
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

        await index.PartialUpdateObjectAsync(productSearchModel);
    }
}
