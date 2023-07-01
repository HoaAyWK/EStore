using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.OrderAggregate.Events;
using EStore.Domain.OrderAggregate.Repositories;
using EStore.Domain.ProductVariantAggregate.Repositories;
using MediatR;

namespace EStore.Application.ProductVariants.Events;

public class OrderRefundedDomainEventHandler : INotificationHandler<OrderRefundedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductVariantRepository _productVariantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderRefundedDomainEventHandler(
        IOrderRepository orderRepository,
        IProductVariantRepository productVariantRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _productVariantRepository = productVariantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(OrderRefundedDomainEvent notification, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(notification.OrderId);

        if (order is null)
        {
            return;
        }

        foreach (var orderItem in order.OrderItems)
        {
            if (orderItem.ItemOrdered.ProductVariantId is not null)
            {
                var productVariant = await _productVariantRepository
                    .GetByIdAsync(orderItem.ItemOrdered.ProductVariantId);

                if (productVariant is not null)
                {
                    productVariant.UpdateStockQuantity(productVariant.StockQuantity + orderItem.Quantity);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
