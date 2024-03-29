using Algolia.Search.Clients;
using EStore.Application.Products.Events;
using EStore.Contracts.Searching;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductVariantUpdatedIntegrationEventHandler
    : INotificationHandler<ProductVariantUpdatedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductVariantUpdatedIntegrationEventHandler(
        IProductRepository productRepository,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> algoliaSearchOptions)
    {
        _productRepository = productRepository;
        _searchClient = searchClient;
        _algoliaSearchOptions = algoliaSearchOptions.Value;
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

        var mainImage = product.Images.Where(image => image.IsMain)
            .First();

        var orderedImages = product.Images
            .OrderBy(image => image.DisplayOrder)
            .ToList();

        var assignedImageIds = productVariant.AssignedProductImageIds.Split(' ');

        if (!assignedImageIds.Contains(mainImage.Id.ToString()))
        {
            // If the main image is not assigned to the product variant, assign the first image
            foreach (var image in orderedImages)
            {
                if (assignedImageIds.Contains(image.Id.ToString()))
                {
                    mainImage = image;
                    break;
                }
            }
        }

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        await index.PartialUpdateObjectAsync(
            new ProductSearchModel
            {
                ObjectID = productVariant.Id.Value.ToString(),
                Image = mainImage.ImageUrl,
                Price = productVariant.Price!.Value,
                IsActive = productVariant.IsActive
            });
    }
}
