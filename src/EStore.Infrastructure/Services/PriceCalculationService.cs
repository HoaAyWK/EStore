using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.DiscountAggregate;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Infrastructure.Services;

public class PriceCalculationService : IPriceCalculationService
{
    public decimal CalculatePrice(
        Product product,
        ProductVariantId? productVariantId,
        Discount? discount)
    {
        decimal variantPrice = 0;

        if (productVariantId is not null)
        {
            var productVariant = product.ProductVariants.FirstOrDefault(v => v.Id == productVariantId);

            variantPrice = productVariant?.Price ?? 0;
        }

        if (HasSpecialPrice(product))
        {
            return product.SpecialPrice!.Value + variantPrice;
        }

        decimal price = product.Price + variantPrice;

        if (HasDiscount(discount))
        {
            if (discount!.UsePercentage)
            {
                price -= price * discount.DiscountPercentage;
            }
            else
            {
                price -= discount.DiscountAmount;
            }
        }

        return price;
    }

    private static bool HasDiscount(Discount? discount)
    {
        return discount is not null &&
                    DateTime.Now >= discount.StartDateTime &&
                    DateTime.UtcNow <= discount.EndDateTime;
    }

    private static bool HasSpecialPrice(Product product)
    {
        return product.SpecialPrice.HasValue &&
                    DateTime.UtcNow >= product.SpecialPriceStartDateTime &&
                    DateTime.UtcNow <= product.SpecialPriceEndDateTime;
    }
}

