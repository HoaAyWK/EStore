using ErrorOr;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Services;

public interface IPriceCalculationService
{
    Task<ErrorOr<decimal>> CalculatePriceAsync(ProductId productId);
}
