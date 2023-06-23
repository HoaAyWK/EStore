using EStore.Domain.CartAggregate.ValueObjects;
using EStore.Domain.CustomerAggregate.ValueObjects;

namespace EStore.Domain.CartAggregate.Repositories;

public interface ICartRepository
{
    Task AddAsync(Cart cart);
    Task<Cart?> GetByIdAsync(CartId id);
    Task<Cart?> GetCartByCustomerId(CustomerId customerId);
    void Delete(Cart cart);
}
