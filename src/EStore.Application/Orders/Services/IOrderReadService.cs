using EStore.Contracts.Common;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.ValueObjects;

namespace EStore.Application.Orders.Services;

public interface IOrderReadService
{
    Task<PagedList<Order>> GetListPagedAsync(int page, int pageSize);
    Task<Order?> GetByIdAsync(OrderId orderId);
    Task<PagedList<Order>> GetByCustomerIdAsync(CustomerId customerId, int page, int pageSize);
}
