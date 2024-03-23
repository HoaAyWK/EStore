using EStore.Application.Orders.Services;
using EStore.Contracts.Common;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.ValueObjects;
using EStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Services;

internal sealed class OrderReadService : IOrderReadService
{
    private readonly EStoreDbContext _dbContext;

    public OrderReadService(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<PagedList<Order>> GetListPagedAsync(int page, int pageSize)
    {
        var totalItems =  await _dbContext.Orders.CountAsync();
        var orders = await _dbContext.Orders.AsNoTracking()
            .Skip((page -1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PagedList<Order>(orders, page, pageSize, totalItems, totalPages);
    }

    public async Task<Order?> GetByIdAsync(OrderId orderId)
    {
        return await _dbContext.Orders.AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<PagedList<Order>> GetByCustomerIdAsync(CustomerId customerId, int page, int pageSize)
    {
        var ordersQuery = _dbContext.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId == customerId);

        var totalItems = await ordersQuery.CountAsync();
        
        var orders = await ordersQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PagedList<Order>(orders, page, pageSize, totalItems, totalPages);
    }
}
