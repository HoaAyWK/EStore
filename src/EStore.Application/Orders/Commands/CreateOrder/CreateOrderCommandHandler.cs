using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Repositories;
using MediatR;

namespace EStore.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
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
        await _unitOfWork.SaveChangesAsync();

        return order;
    }
}
