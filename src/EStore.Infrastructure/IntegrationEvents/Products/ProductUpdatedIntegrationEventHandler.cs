using Algolia.Search.Clients;
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
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductUpdatedIntegrationEventHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        IDiscountRepository discountRepository,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _discountRepository = discountRepository;
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

        if (product.HasVariant)
        {
            Discount? discount = null;
            var productSearchModels = new List<ProductSearchModel>();

            if (product.DiscountId is not null)
            {
                discount = await _discountRepository.GetByIdAsync(product.DiscountId);
            }

            foreach (var variant in product.ProductVariants)
            {
                var model = new ProductSearchModel
                {
                    ObjectID = variant.Id.Value.ToString(),
                    ProductId = product.Id.Value,
                    ProductVariantId = variant.Id.Value,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Categories = hierarchyCategories.ToList(),
                    Brand = brand?.Name,
                    SpecialPrice = product.SpecialPrice,
                    SpecialPriceStartDateTime = product.SpecialPriceStartDateTime,
                    SpecialPriceEndDateTime = product.SpecialPriceEndDateTime,
                    AverageRating = product.AverageRating.Value,
                    DisplayOrder = product.DisplayOrder,
                    CreatedDateTime = product.CreatedDateTime,
                    UpdatedDateTime = product.UpdatedDateTime,
                    Image = product.Images
                        .Where(image => image.IsMain)
                        .Select(image => image.ImageUrl)
                        .FirstOrDefault() ?? string.Empty
                };

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
            SpecialPrice = product.SpecialPrice,
            SpecialPriceStartDateTime = product.SpecialPriceStartDateTime,
            SpecialPriceEndDateTime = product.SpecialPriceEndDateTime,
            AverageRating = product.AverageRating.Value,
            DisplayOrder = product.DisplayOrder,
            CreatedDateTime = product.CreatedDateTime,
            UpdatedDateTime = product.UpdatedDateTime,
            Image = product.Images
                .Where(image => image.IsMain)
                .Select(image => image.ImageUrl)
                .FirstOrDefault() ?? string.Empty
        };

        await index.PartialUpdateObjectAsync(productSearchModel);
    }
}
