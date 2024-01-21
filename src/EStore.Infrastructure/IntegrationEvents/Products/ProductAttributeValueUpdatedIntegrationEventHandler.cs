using Algolia.Search.Clients;
using EStore.Application.Products.Events;
using EStore.Contracts.Searching;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductAttributeValueUpdatedIntegrationEventHandler
    : INotificationHandler<ProductAttributeValueUpdatedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductAttributeValueUpdatedIntegrationEventHandler(
        IProductRepository productRepository,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options)
    {
        _productRepository = productRepository;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
    }

    public async Task Handle(
        ProductAttributeValueUpdatedIntegrationEvent notification,
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

        var productAttribute = product.ProductAttributes.SingleOrDefault(
            x => x.Id == notification.ProductAttributeId);

        if (productAttribute is null)
        {
            // TODO: log error

            return;
        }

        var productAttributeValue = productAttribute.ProductAttributeValues.SingleOrDefault(
            x => x.Id == notification.ProductAttributeValueId);

        if (productAttributeValue is null)
        {
            // TODO: log error

            return;
        }

        var attributesOfProductSearchModel = new Dictionary<string, string>();

        foreach (var attribute in product.ProductAttributes)
        {
            if (attribute.CanCombine)
            {
                continue;
            }

            var attributeValue = attribute.ProductAttributeValues
                .ToList()
                .FirstOrDefault();

            if (attributeValue is not null)
            {
                attributesOfProductSearchModel.Add(attribute.Name, attributeValue.Name);
            }
        }

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        await index.PartialUpdateObjectAsync(
            new ProductSearchModel
            {
                ObjectID = product.Id.Value.ToString(),
                Attributes = attributesOfProductSearchModel
            });
    }
}
