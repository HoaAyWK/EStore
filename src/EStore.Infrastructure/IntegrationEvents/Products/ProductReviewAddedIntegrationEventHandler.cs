using System.Dynamic;
using Algolia.Search.Clients;
using EStore.Application.Products.Events;
using EStore.Contracts.Searching;
using EStore.Domain.Common.Utilities;
using EStore.Domain.DiscountAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductReviewAddedIntegrationEventHandler : INotificationHandler<ProductUpdatedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductReviewAddedIntegrationEventHandler(
        IProductRepository productRepository,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options)
    {
        _productRepository = productRepository;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
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

            await index.PartialUpdateObjectAsync(
                new ProductSearchModel
                {
                    ObjectID = variant.Id.Value.ToString(),
                    AverageRating = variant.AverageRating.Value,
                    NumRatings = variant.AverageRating.NumRatings
                },
                ct: cancellationToken);

            return;
        }

        await index.PartialUpdateObjectAsync(
            new ProductSearchModel
            {
                ObjectID = product.Id.Value.ToString(),
                AverageRating = product.AverageRating.Value,
                NumRatings = product.AverageRating.NumRatings
            },
            ct: cancellationToken);
    }
}
