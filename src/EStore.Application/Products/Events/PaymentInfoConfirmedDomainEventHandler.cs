using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.OrderAggregate.Events;
using EStore.Domain.OrderAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using MediatR;

namespace EStore.Application.Products.Events;

public class PaymentInfoConfirmedDomainEventHandler
    : INotificationHandler<PaymentInfoConfirmedDomainEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentInfoConfirmedDomainEventHandler(
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        PaymentInfoConfirmedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(notification.OrderId);

        if (order is null)
        {
            // TODO: log error

            return;
        }

        foreach (var orderItem in order.OrderItems)
        {
            var product = await _productRepository.GetByIdAsync(orderItem.ItemOrdered.ProductId);

            if (product is null)
            {
                // TODO: log error
                continue;
            }

            if (!product.HasVariant)
            {
                var remainingStockQuantity = product.StockQuantity - orderItem.Quantity;

                if (remainingStockQuantity < 0)
                {
                    // TODO: log error
                    continue;
                }

                product.UpdateStockQuantity(remainingStockQuantity);

                continue;
            }

            if (orderItem.ItemOrdered.ProductVariantId is not null)
            {
                var productVariant = product.ProductVariants
                    .Where(variant => variant.Id == orderItem.ItemOrdered.ProductVariantId)
                    .FirstOrDefault();

                if (productVariant is null)
                {
                    continue;
                }

                var remainingProductVariantStockQuantity = productVariant.StockQuantity - orderItem.Quantity;

                if (remainingProductVariantStockQuantity < 0)
                {
                    continue;
                }

                productVariant.UpdateStockQuantity(remainingProductVariantStockQuantity);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
