using Algolia.Search.Clients;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Products.Events;
using EStore.Contracts.Searching;
using EStore.Domain.DiscountAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.IntegrationEvents.Products;

public class ProductDiscountAssignedIntegrationEvenHandler
    : INotificationHandler<ProductDiscountAssignedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IPriceCalculationService _priceCalculationService;
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public ProductDiscountAssignedIntegrationEvenHandler(
        IProductRepository productRepository,
        IDiscountRepository discountRepository,
        IPriceCalculationService priceCalculationService,
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> algoliaSearchOptions)
    {
        _productRepository = productRepository;
        _discountRepository = discountRepository;
        _priceCalculationService = priceCalculationService;
        _searchClient = searchClient;
        _algoliaSearchOptions = algoliaSearchOptions.Value;
    }

    public async Task Handle(
        ProductDiscountAssignedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId);

        if (product is null)
        {
            // TODO: log errors

            return;
        }

        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        if (notification.DiscountId is not null)
        {
            var discount = await _discountRepository.GetByIdAsync(notification.DiscountId);

            if (discount is null)
            {
                // TODO: log errors

                return;
            }

            var productSearchDiscount = new ProductSearchDiscount
            {
                DiscountName = discount.Name,
                UsePercentage = discount.UsePercentage,
                DiscountPercentage = discount.DiscountPercentage,
                DiscountAmount = discount.DiscountAmount,
                StartDateTime = discount.StartDateTime,
                EndDateTime = discount.EndDateTime
            };

            if (!product.HasVariant && product.Published)
            {
                var productSearchModel = await index.GetObjectAsync<ProductSearchModel>(
                    product.Id.Value.ToString());

                productSearchModel.Discount = productSearchDiscount;
                productSearchModel.FinalPrice = _priceCalculationService.ApplyDiscount(
                    product.Price,
                    discount);

                await index.SaveObjectAsync(productSearchModel, ct: cancellationToken);

                return;
            }

            var productVariantIds = product.ProductVariants
                .Select(x => x.Id.Value.ToString())
                .ToArray();

            var productSearchModels = await index.GetObjectsAsync<ProductSearchModel>(
                productVariantIds);

            foreach (var model in productSearchModels)
            {
                var variant = product.ProductVariants.Where(variant =>
                    variant.Id == ProductVariantId.Create(new Guid(model.ObjectID)))
                    .SingleOrDefault();

                model.Discount = productSearchDiscount;
                model.FinalPrice = _priceCalculationService.ApplyDiscount(
                    model.Price,
                    discount);
            }

            await index.SaveObjectsAsync(productSearchModels);

            return;
        }

        if (!product.HasVariant && product.Published)
        {
            var singleProduct = await index.GetObjectAsync<ProductSearchModel>(
                product.Id.Value.ToString());

            singleProduct.Discount = null;
            singleProduct.FinalPrice = product.Price;

            await index.SaveObjectAsync(singleProduct, ct: cancellationToken);

            return;
        }

        var variantIds = product.ProductVariants
            .Where(x => x.IsActive)
            .Select(x => x.Id.Value.ToString())
            .ToArray();

        var searchModels = await index.GetObjectsAsync<ProductSearchModel>(variantIds);

        foreach (var model in searchModels)
        {
            model.Discount = null;
            model.FinalPrice = model.Price;
        }

        await index.SaveObjectsAsync(searchModels);
    }
}
