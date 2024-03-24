using Algolia.Search.Clients;
using EStore.Application.Products.Events;
using EStore.Contracts.Searching;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductImageAddedIntegrationEventHandler
    : INotificationHandler<ProductImageAddedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductImageAddedIntegrationEventHandler(
        IProductRepository productRepository,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> algoliaSearchOptions)
    {
        _productRepository = productRepository;
        _searchClient = searchClient;
        _algoliaSearchOptions = algoliaSearchOptions.Value;
    }

    public async Task Handle(
        ProductImageAddedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            return;
        }

        var image =  product.Images.FirstOrDefault(i =>
            i.Id == notification.ProductImageId);

        if (image is null)
        {
            return;
        }

        if (!image.IsMain)
        {
            return;
        }

        if (!product.HasVariant)
        {
            var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

            await index.PartialUpdateObjectAsync(
                new ProductSearchModel
                {
                    ObjectID = product.Id.Value.ToString(),
                    Image = image.ImageUrl
                });

            return;
        }
    }
}