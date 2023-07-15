using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Repositories;
using MediatR;

namespace EStore.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(
            request.CustomerId,
            request.OrderStatus,
            request.TransactionId,
            request.ShippingAddress,
            request.OrderItems);

        await _orderRepository.AddAsync(order);

        return order;
    }
}
