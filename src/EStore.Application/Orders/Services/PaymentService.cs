using EStore.Contracts.Carts;

namespace EStore.Application.Orders.Services;

public interface IPaymentService
{
    Task<string> ProcessPaymentAsync(CartResponse cart);
}
