using EStore.Domain.DiscountAggregate;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Application.Common.Interfaces.Services;

public interface IPriceCalculationService
{
    decimal CalculatePrice(
        Product product,
        ProductVariantId? productVariantId,
        Discount? discount);
}
