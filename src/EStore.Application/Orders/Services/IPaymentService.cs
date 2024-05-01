using EStore.Domain.OrderAggregate;

namespace EStore.Application.Orders.Services;

public interface IPaymentService
{
    Task<string> ProcessPaymentAsync(
        Order order,
        CancellationToken cancellationToken = default);
        
    Task ProcessRefundAsync(
        Order order,
        CancellationToken cancellationToken = default);
}
