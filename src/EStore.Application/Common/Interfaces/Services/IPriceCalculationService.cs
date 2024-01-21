using EStore.Domain.DiscountAggregate;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Entities;

namespace EStore.Application.Common.Interfaces.Services;

public interface IPriceCalculationService
{
    decimal CalculatePrice(
        Product product,
        ProductVariant? productVariant);

    decimal ApplyDiscount(decimal price, Discount discount);
}
