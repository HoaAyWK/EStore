using Algolia.Search.Clients;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Products.Events;
using EStore.Application.Products.Services;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Utilities;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Infrastructure.Services.AlgoliaSearch.Models;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductVariantCreatedIntegrationEventHandler
    : INotificationHandler<ProductVariantCreatedIntegrationEvent>
{
    private readonly IProductReadService _productReadService;
    private readonly ICategoryReadService _categoryReadService;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductVariantCreatedIntegrationEventHandler(
        IProductReadService productReadService,
        ICategoryReadService categoryReadService,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options)
    {
        _productReadService = productReadService;
        _categoryReadService = categoryReadService;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
    }

    public async Task Handle(
        ProductVariantCreatedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productReadService.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            return;
        }

        var productVariant = product.Variants.FirstOrDefault(
            v => v.Id == notification.ProductVariantId.Value);

        if (productVariant is null)
        {
            return;
        }

        var attributeSelection = AttributeSelection<ProductAttributeId, ProductAttributeValueId>
            .Create(productVariant.RawAttributeSelection);
    
        var price = product.Price;

        var productRecord = new ProductRecord
        {
            ObjectID = notification.ProductVariantId.Value.ToString(),
            ProductVariantId = notification.ProductVariantId.Value,
            ProductId = notification.ProductId.Value,
            Name = product.Name,
            Description = product.Description,
            SpecialPrice = product.SpecialPrice,
            SpecialPriceStartDateTime = product.SpecialPriceStartDate,
            SpecialPriceEndDateTime = product.SpecialPriceEndDate,
            StockQuantity = productVariant.StockQuantity,
            AverageRating = product.AverageRating.Value,
            DisplayOrder = product.DisplayOrder,
            CreatedDateTime = product.CreatedDateTime,
            UpdatedDateTime = product.UpdatedDateTime,
            IsActive = productVariant.IsActive,
            Brand = product.Brand?.Name,
            Image = product.Images
                .Where(image => image.IsMain)
                .Select(image => image.ImageUrl)
                .FirstOrDefault() ?? string.Empty
        };

        foreach (var selection in attributeSelection.AttributesMap)
        {
            var attribute = product.Attributes.FirstOrDefault(
                x => x.Id == selection.Key.Value);

            if (attribute is null)
            {
                return;
            }

            var attributeValue = attribute.AttributeValues.FirstOrDefault(
                x => x.Id == selection.Value.First().Value);

            if (attributeValue is null)
            {
                return;
            }

            price += attributeValue.PriceAdjustment;
            
            if (attribute.Name == nameof(ProductRecord.Color))
            {
                productRecord.Color = attributeValue.Name;
            }
            else if (attribute.Name == nameof(ProductRecord.Storage))
            {
                productRecord.Storage = attributeValue.Name;
            }
            else if (attribute.Name == nameof(ProductRecord.Memory))
            {
                productRecord.Memory = attributeValue.Name;
            }
        }

        productRecord.Price = price;

        var category = await _categoryReadService.GetByIdAsync(
                CategoryId.Create(product!.Category!.Id));

        var hierarchyCategories = new LinkedList<string>();

        if (category is not null)
        {
            var current = category;

            while (current is not null)
            {
                hierarchyCategories.AddFirst(current.Name);
                current = current.Parent;
            }
        }

        productRecord.Categories = hierarchyCategories.ToList();

        if (product.Discount is not null)
        {
            productRecord.Discount = new DiscountRecord
            {
                UsePercentage = product.Discount.UsePercentage,
                Percentage = product.Discount.Percentage,
                Amount = product.Discount.Amount,
                StartDate = product.Discount.StartDate,
                EndDate = product.Discount.EndDate
            };
        }

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        await index.SaveObjectAsync(productRecord);
    }
}
