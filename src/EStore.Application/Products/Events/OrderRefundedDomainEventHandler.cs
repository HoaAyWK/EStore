using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.OrderAggregate.Events;
using EStore.Domain.OrderAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using MediatR;

namespace EStore.Application.Products.Events;

public class OrderRefundedDomainEventHandler : INotificationHandler<OrderRefundedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderRefundedDomainEventHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
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
            var product = await _productRepository.GetByIdAsync(orderItem.ItemOrdered.ProductId);

            if (product is not null)
            {
                if (orderItem.ItemOrdered.ProductVariantId is null)
                {
                    product.UpdateStockQuantity(product.StockQuantity + orderItem.Quantity);
                }
                else
                {
                    var productVariant =  product.ProductVariants
                        .FirstOrDefault(v => v.Id == orderItem.ItemOrdered.ProductVariantId);
                    
                    productVariant!.UpdateStockQuantity(productVariant.StockQuantity + orderItem.Quantity);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
