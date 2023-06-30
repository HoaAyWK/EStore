using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Repositories;
using EStore.Domain.OrderAggregate.ValueObjects;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly EStoreDbContext _dbContext;

    public OrderRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
    }

    public async Task<Order?> GetByIdAsync(OrderId id)
    {
        return await _dbContext.Orders.FindAsync(id);
    }
}
