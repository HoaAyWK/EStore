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

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);
        var attributesOfProductSearchModel = new Dictionary<string, string>();

        foreach (var attribute in product.ProductAttributes)
        {
            if (attribute.CanCombine)
            {
                continue;
            }

            var attributeValue = attribute.ProductAttributeValues.FirstOrDefault();

            if (attributeValue is not null)
            {
                attributesOfProductSearchModel.Add(attribute.Name, attributeValue.Name);
            }
        }

        if (product.HasVariant)
        {
            var productSearchModels = new List<ProductSearchModel>();

            foreach (var variant in product.ProductVariants)
            {
                var productSearchModel = new ProductSearchModel
                {
                    ObjectID = variant.Id.Value.ToString()
                };

                var productSearchModelAttributes = attributesOfProductSearchModel.ToDictionary(
                    x => x.Key,
                    x => x.Value);

                var attributeSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
                    .Create(variant.RawAttributeSelection);

                foreach (var selection in attributeSelection.AttributesMap)
                {
                    var attribute = product.ProductAttributes.FirstOrDefault(
                        x => x.Id == selection.Key);

                    if (attribute is null)
                    {
                        // TODO: log error

                        return;
                    }

                    var attributeValue = attribute.ProductAttributeValues.FirstOrDefault(
                        x => x.Id == selection.Value.First());

                    if (attributeValue is null)
                    {
                        // TODO: log error

                        return;
                    }
                    
                    productSearchModelAttributes.Add(attribute.Name, attributeValue.Name);
                }

                productSearchModel.Attributes = productSearchModelAttributes;
                productSearchModels.Add(productSearchModel);
            }

            await index.PartialUpdateObjectsAsync(productSearchModels);

            return;
        }

        await index.PartialUpdateObjectAsync(
            new ProductSearchModel
            {
                ObjectID = product.Id.Value.ToString(),
                Attributes = attributesOfProductSearchModel
            });
    }
}
