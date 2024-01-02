using Algolia.Search.Clients;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Products.Events;
using EStore.Application.Products.Services;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Infrastructure.Services.AlgoliaSearch.Models;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductCreatedIntegrationEventHandler
    : INotificationHandler<ProductCreatedIntegrationEvent>
{
    private readonly IProductReadService _productReadService;
    private readonly ICategoryReadService _categoryReadService;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductCreatedIntegrationEventHandler(
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
        ProductCreatedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productReadService.GetByIdAsync(notification.ProductId);

        if (product is not null)
        {
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

            var productRecord = new ProductRecord
            {
                ObjectID = product.Id.ToString(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Categories = hierarchyCategories.ToList(),
                Brand = product.Brand!.Name,
                SpecialPrice = product.SpecialPrice,
                SpecialPriceStartDateTime = product.SpecialPriceStartDate,
                SpecialPriceEndDateTime = product.SpecialPriceEndDate,
                AverageRating = product.AverageRating.Value,
                DisplayOrder = product.DisplayOrder,
                CreatedDateTime = product.CreatedDateTime,
                UpdatedDateTime = product.UpdatedDateTime,
                Image = product.Images
                    .Where(image => image.IsMain)
                    .Select(image => image.ImageUrl)
                    .FirstOrDefault() ?? string.Empty
            };

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
}
