using Algolia.Search.Clients;
using EStore.Application.Products.Events;
using EStore.Contracts.Searching;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductAttributeValueRemovedIntegrationEventHandler
    : INotificationHandler<ProductAttributeValueRemovedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductAttributeValueRemovedIntegrationEventHandler(
        IProductRepository productRepository,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options)
    {
        _productRepository = productRepository;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
    }

    public async Task Handle(
        ProductAttributeValueRemovedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            // TODO: log error

            return;
        }

        if (product.HasVariant && product.ProductVariants.Count > 0)
        {
            return;
        }

        var attribute = product.ProductAttributes.SingleOrDefault(
            x => x.Id == notification.ProductAttributeId);

        if (attribute is null)
        {
            // TODO: log error

            return;
        }

        // No need to update Algolia index if the attribute can be combined
        if (attribute.CanCombine)
        {
            return;
        }

        var attributeValue = attribute.ProductAttributeValues.SingleOrDefault(
            x => x.Id == notification.ProductAttributeValueId);

        if (attributeValue is null)
        {
            // TODO: log error

            return;
        }

        var attributesOfProductSearchModel = new Dictionary<string, string>();

        foreach (var productAttribute in product.ProductAttributes)
        {
            if (productAttribute.CanCombine)
            {
                continue;
            }

            var productAttributeValue = productAttribute.ProductAttributeValues
                .SingleOrDefault(x => x.Id == notification.ProductAttributeValueId);

            if (productAttributeValue is null)
            {
                continue;
            }

            attributesOfProductSearchModel.Add(
                productAttribute.Name,
                productAttributeValue.Name);
        }

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);
        var productSearchModel = await index.GetObjectAsync<ProductSearchModel>(
            product.Id.Value.ToString());

        productSearchModel.Attributes = attributesOfProductSearchModel;

        await index.SaveObjectAsync(productSearchModel);
    }
}

