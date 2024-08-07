using Algolia.Search.Clients;
using EStore.Application.Products.Events;
using EStore.Contracts.Searching;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductReviewAddedIntegrationEventHandler
    : INotificationHandler<ProductReviewAddedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;
    private readonly ILogger<ProductReviewAddedIntegrationEventHandler> _logger;

    public ProductReviewAddedIntegrationEventHandler(
        IProductRepository productRepository,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options,
        ILogger<ProductReviewAddedIntegrationEventHandler> logger)
    {
        _productRepository = productRepository;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
        _logger = logger;
    }

    public async Task Handle(
        ProductReviewAddedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            // TODO: log

            return;
        }

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        ProductSearchModel? productSearchModel = null;

        if (notification.ProductVariantId is not null)
        {
            var variant = product.ProductVariants
                .Where(variant => variant.Id == notification.ProductVariantId)
                .FirstOrDefault();

            if (variant is null)
            {
                // TODO: log

                return;
            }

            if (!variant.IsActive)
            {
                return;
            }

            try
            {
                productSearchModel = await index.GetObjectAsync<ProductSearchModel>(
                    variant.Id.Value.ToString());
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to get product variant from search index");
            }

            if (productSearchModel is not null)
            {
                productSearchModel.AverageRating = variant.AverageRating.Value;
                productSearchModel.NumRatings = variant.AverageRating.NumRatings;
                await index.SaveObjectAsync(productSearchModel);
            }

            return;
        }

        try
        {
            productSearchModel = await index.GetObjectAsync<ProductSearchModel>(
                product.Id.Value.ToString());
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to get product from search index");
        }

        if (productSearchModel is not null)
        {
            productSearchModel.AverageRating = product.AverageRating.Value;
            productSearchModel.NumRatings = product.AverageRating.NumRatings;
            await index.PartialUpdateObjectAsync(productSearchModel);
        }
    }
}
