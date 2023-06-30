using ErrorOr;
using EStore.Contracts.Carts;
using EStore.Domain.CustomerAggregate.ValueObjects;

namespace EStore.Application.Carts.Services;

public interface ICartReadService
{
    Task<CartResponse?> GetByCustomerIdAsync(CustomerId customerId);
    Task<ErrorOr<Success>> ValidatePurchasedItemsAsync(CustomerId customerId);
}
