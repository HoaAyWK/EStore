using System.Dynamic;
using Algolia.Search.Clients;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Products.Events;
using EStore.Contracts.Searching;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.DiscountAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductCreatedIntegrationEventHandler
    : INotificationHandler<ProductCreatedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IPriceCalculationService _priceCalculationService;
    private readonly IHierarchicalCategoryService _hierarchicalCategoryService;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductCreatedIntegrationEventHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        IDiscountRepository discountRepository,
        IPriceCalculationService priceCalculationService,
        IHierarchicalCategoryService hierarchicalCategoryService,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _discountRepository = discountRepository;
        _priceCalculationService = priceCalculationService;
        _hierarchicalCategoryService = hierarchicalCategoryService;
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
    }

    public async Task Handle(
        ProductCreatedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            // TODO: log

            return;
        }

        if (product.HasVariant)
        {
            return;
        }

        if (!product.Published)
        {
            return;
        }

        var brand = await _brandRepository.GetByIdAsync(product.BrandId);
        var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
        var hierarchyCategories = new ExpandoObject();

        if (category is not null)
        {
            hierarchyCategories = _hierarchicalCategoryService.GetHierarchy(category);
        }

        var productSearchModel = new ProductSearchModel
        {
            ObjectID = product.Id.Value.ToString(),
            ProductId = product.Id.Value,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            FinalPrice = product.Price,
            Categories = hierarchyCategories.ToList(),
            Brand = brand?.Name,
            AverageRating = product.AverageRating.Value,
            DisplayOrder = product.DisplayOrder,
            CreatedDateTime = product.CreatedDateTime,
            UpdatedDateTime = product.UpdatedDateTime,
            HasVariant = product.HasVariant,            
            Image = product.Images
                .Where(image => image.IsMain)
                .Select(image => image.ImageUrl)
                .FirstOrDefault() ?? string.Empty
        };

        if (product.DiscountId is not null)
        {
            var discount = await _discountRepository.GetByIdAsync(product.DiscountId);

            if (discount is not null)
            {
                productSearchModel.Discount = new ProductSearchDiscount
                {
                    UsePercentage = discount.UsePercentage,
                    DiscountPercentage = discount.DiscountPercentage,
                    DiscountAmount = discount.DiscountAmount,
                    StartDateTime = discount.StartDateTime,
                    EndDateTime = discount.EndDateTime
                };

                productSearchModel.FinalPrice = _priceCalculationService.ApplyDiscount(product.Price, discount);
            }
        }

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        await index.SaveObjectAsync(productSearchModel);
    }
}
