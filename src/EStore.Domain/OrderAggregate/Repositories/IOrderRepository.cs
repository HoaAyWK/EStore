using EStore.Domain.OrderAggregate.ValueObjects;

namespace EStore.Domain.OrderAggregate.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(OrderId id);
    Task<Order?> GetByTransactionIdAsync(string transactionId); 
}
