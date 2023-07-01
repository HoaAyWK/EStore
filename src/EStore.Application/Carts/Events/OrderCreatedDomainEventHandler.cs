using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.CartAggregate.Repositories;
using EStore.Domain.OrderAggregate.Events;
using MediatR;

namespace EStore.Application.Carts.Events;

public class OrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderCreatedDomainEventHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartByCustomerId(notification.CustomerId);

        if (cart is null)
        {
            return;
        }

        cart.Clear();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
