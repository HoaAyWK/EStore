using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.DiscountAggregate;
using EStore.Domain.DiscountAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.Services;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Application.Products.Services;

public class PriceCalculationService : IPriceCalculationService
{
    private readonly IProductRepository _productRepository;
    private readonly IDiscountRepository _discountRepository;

    public PriceCalculationService(
        IProductRepository productRepository,
        IDiscountRepository discountRepository)
    {
        _productRepository = productRepository;
        _discountRepository = discountRepository;
    }

    public async Task<ErrorOr<decimal>> CalculatePriceAsync(ProductId productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        if (product.SpecialPrice.HasValue && product.SpecialPriceEndDateTime > DateTime.UtcNow)
        {
            return product.SpecialPrice.Value;
        }

        var price = product.Price;

        if (product.DiscountId is not null)
        {
            var discount = await _discountRepository.GetByIdAsync(product.DiscountId);

            if (IsDiscountValid(discount))
            {
                if (discount!.UsePercentage)
                {
                    price = Math.Ceiling(price - (price * discount.DiscountPercentage) * 100) / 100;
                }
                else
                {
                    price = price - discount.DiscountAmount;
                }
            }
        }

        return price;
    }

    private static bool IsDiscountValid(Discount? discount)
    {
        return discount is not null &&
            DateTime.UtcNow >= discount.StartDateTime &&
            DateTime.UtcNow <= discount.EndDateTime;
    }
}
