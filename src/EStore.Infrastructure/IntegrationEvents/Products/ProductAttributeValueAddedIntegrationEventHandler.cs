using Algolia.Search.Clients;
using Algolia.Search.Models.Settings;
using EStore.Contracts.Searching;
using EStore.Domain.ProductAggregate.Events;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Infrastructure.Services.AlgoliaSearch.Interfaces;
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
    private readonly IAlgoliaIndexSettingsService _algoliaIndexSettingsService;

    public ProductAttributeValueAddedIntegrationEventHandler(
        IProductRepository productRepository,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options,
        IAlgoliaIndexSettingsService algoliaIndexSettingsService)
    {
        _productRepository = productRepository;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
        _algoliaIndexSettingsService = algoliaIndexSettingsService;
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

        var searchIndexSettings = await _algoliaIndexSettingsService.GetIndexSettingsAsync();
        var attributesForFaceting = searchIndexSettings?.AttributesForFaceting.ToHashSet()
            ?? new HashSet<string>();

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        if (product.HasVariant)
        {
            var variantIds = product.ProductVariants
                .Select(x => x.Id.Value.ToString())
                .ToList();

            var models = await index.GetObjectsAsync<ProductSearchModel>(
                variantIds);

            foreach (var model in models)
            {
                model.Attributes
                    .Add(productAttribute.Name, productAttributeValue.Name);

                if (!attributesForFaceting.Contains(productAttribute.Name))
                {
                    attributesForFaceting.Add(productAttribute.Name);
                }
            }

            await index.PartialUpdateObjectsAsync(models);
            await index.SetSettingsAsync(new IndexSettings
            {
                AttributesForFaceting = attributesForFaceting.ToList()
            });

            return;
        }

        var productSearchModel = await index.GetObjectAsync<ProductSearchModel>(
            product.Id.Value.ToString());

        if (productSearchModel is null)
        {
            // TODO: log error

            return;
        }

        productSearchModel.Attributes
            .Add(productAttribute.Name, productAttributeValue.Name);
        
        if (!attributesForFaceting.Contains(productAttribute.Name))
        {
            attributesForFaceting.Add(productAttribute.Name);
        }

        await index.PartialUpdateObjectAsync(productSearchModel);
        await index.SetSettingsAsync(new IndexSettings
        {
            AttributesForFaceting = attributesForFaceting.ToList()
        });
    }
}
