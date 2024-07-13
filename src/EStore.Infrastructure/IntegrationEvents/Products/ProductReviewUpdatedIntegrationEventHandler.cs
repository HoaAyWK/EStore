using Algolia.Search.Clients;
using EStore.Application.Products.Events;
using EStore.Contracts.Searching;
using EStore.Domain.Common.Utilities;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;


namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductReviewUpdatedIntegrationEventHandler
    : INotificationHandler<ProductReviewUpdatedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;
    private readonly ILogger<ProductReviewUpdatedIntegrationEventHandler> _logger;

    public ProductReviewUpdatedIntegrationEventHandler(
        IProductRepository productRepository,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options,
        ILogger<ProductReviewUpdatedIntegrationEventHandler> logger)
    {
        _productRepository = productRepository;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
        _logger = logger;
    }

    public async Task Handle(ProductReviewUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            // TODO: log

            return;
        }

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        ProductSearchModel? productSearchModel = null;

        var productReview = product.ProductReviews
            .Where(review => review.Id == notification.ProductReviewId)
            .SingleOrDefault();

        if (productReview is null)
        {
            return;
        }

        var productVariant = product.ProductVariants.Where(variant =>
            AttributeSelection<ProductAttributeId, ProductAttributeValueId>.Create(variant.RawAttributeSelection).Equals(
                AttributeSelection<ProductAttributeId, ProductAttributeValueId>.Create(productReview.RawAttributeSelection)))
            .SingleOrDefault();

        if (productVariant is not null)
        {
            if (!productVariant.IsActive)
            {
                return;
            }

            try
            {
                productSearchModel = await index.GetObjectAsync<ProductSearchModel>(
                    productVariant.Id.Value.ToString());
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to get product variant from search index");
            }

            if (productSearchModel is not null)
            {
                productSearchModel.AverageRating = productVariant.AverageRating.Value;
                productSearchModel.NumRatings = productVariant.AverageRating.NumRatings;
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
            await index.SaveObjectAsync(productSearchModel);
        }
    }
}