using EStore.Domain.CartAggregate;
using EStore.Domain.CartAggregate.Repositories;
using EStore.Domain.CartAggregate.ValueObjects;
using EStore.Domain.CustomerAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class CartRepository : ICartRepository
{
    private readonly EStoreDbContext _dbContext;

    public CartRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Cart cart)
    {
        await _dbContext.Carts.AddAsync(cart);
    }

    public async Task<Cart?> GetByIdAsync(CartId id)
    {
        return await _dbContext.Carts.FindAsync(id);
    }

     public async Task<Cart?> GetCartByCustomerId(CustomerId customerId)
    {
        return await _dbContext.Carts
            .Where(c => c.CustomerId == customerId)
            .FirstOrDefaultAsync();
    }

    public void Delete(Cart cart)
    {
        _dbContext.Remove(cart);
    }
}
