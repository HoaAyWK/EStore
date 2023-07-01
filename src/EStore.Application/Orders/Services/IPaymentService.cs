using EStore.Domain.OrderAggregate;

namespace EStore.Application.Orders.Services;

public interface IPaymentService
{
    Task<string> ProcessPaymentAsync(Order order);
    Task ProcessRefundAsync(Order order);
}
