using Algolia.Search.Clients;
using EStore.Application.Products.Events;
using EStore.Contracts.Searching;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductCreatedIntegrationEventHandler
    : INotificationHandler<ProductCreatedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductCreatedIntegrationEventHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
    }

    public async Task Handle(
        ProductCreatedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            // TODO: log

            return;
        }

        if (product.HasVariant)
        {
            return;
        }

        if (!product.Published)
        {
            return;
        }

        var brand = await _brandRepository.GetByIdAsync(product.BrandId);
        var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
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
            HasVariant = product.HasVariant,            
            Image = product.Images
                .Where(image => image.IsMain)
                .Select(image => image.ImageUrl)
                .FirstOrDefault() ?? string.Empty
        };

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        await index.SaveObjectAsync(productSearchModel);
    }
}
