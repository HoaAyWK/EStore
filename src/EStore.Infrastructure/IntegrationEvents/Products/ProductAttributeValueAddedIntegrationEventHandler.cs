using Algolia.Search.Clients;
using EStore.Contracts.Searching;
using EStore.Domain.Common.Utilities;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductAttributeValueAddedIntegrationEventHandler
    : INotificationHandler<ProductAttributeValueAddedDomainEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductAttributeValueAddedIntegrationEventHandler(
        IProductRepository productRepository,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options)
    {
        _productRepository = productRepository;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
    }

    public async Task Handle(
        ProductAttributeValueAddedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            // TODO: log error

            return;
        }

        var productAttribute = product.ProductAttributes.SingleOrDefault(
            x => x.Id == notification.ProductAttributeId);

        if (productAttribute is null)
        {
            // TODO: log error

            return;
        }

        if (productAttribute.CanCombine)
        {
            return;
        }

        var productAttributeValue = productAttribute.ProductAttributeValues.SingleOrDefault(
            x => x.Id == notification.ProductAttributeValueId);

        if (productAttributeValue is null)
        {
            // TODO: log error

            return;
        }

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        if (product.HasVariant)
        {
            var variantIds = product.ProductVariants
                .Select(x => x.Id.Value.ToString())
                .ToList();

            var models = await index.GetObjectsAsync<ProductSearchModel>(
                variantIds,
                attributesToRetrieve: new[] { "attributes" });

            foreach (var model in models)
            {
                model?.Attributes
                    .Add(productAttribute.Name, productAttributeValue.Name);
            }

            await index.PartialUpdateObjectsAsync(models);

            return;
        }

        var productSearchModel = await index.GetObjectAsync<ProductSearchModel>(
            product.Id.Value.ToString(),
            attributesToRetrieve: new[] { "attributes" });

        if (productSearchModel is null)
        {
            // TODO: log error

            return;
        }

        productSearchModel.Attributes
            .Add(productAttribute.Name, productAttributeValue.Name);

        await index.PartialUpdateObjectAsync(productSearchModel);
    }
}
