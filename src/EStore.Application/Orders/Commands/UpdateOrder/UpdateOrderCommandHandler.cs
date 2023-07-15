using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductVariantAggregate.Repositories;
using MediatR;

namespace EStore.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler
    : IRequestHandler<UpdateOrderCommand, ErrorOr<Updated>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductVariantRepository _productVariantRepository;

    public UpdateOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IProductVariantRepository productVariantRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _productVariantRepository = productVariantRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return Errors.Order.NotFound;
        }

        if (order.OrderStatus == OrderStatus.Cancelled)
        {
            return Errors.Order.CannotUpdate(order.Id);
        }

        order.UpdateOrderStatus(request.OrderStatus);
        
        if (request.ShippingAddress is not null)
        {
            order.UpdateShippingAddress(request.ShippingAddress);
        }

        if (request.TransactionId is not null)
        {
            order.UpdateTransactionId(request.TransactionId);
        }

        foreach (var orderItem in order.OrderItems)
        {
            if (orderItem.ItemOrdered.ProductVariantId is not null)
            {
                var productVariant = await _productVariantRepository
                    .GetByIdAsync(orderItem.ItemOrdered.ProductVariantId);

                if (productVariant is null)
                {
                    return Errors.Order.ProductVariantNotFound(orderItem.ItemOrdered.ProductVariantId);
                }

                productVariant.UpdateStockQuantity(productVariant.StockQuantity - orderItem.Quantity);
            }
            else
            {
                var product = await _productRepository.GetByIdAsync(orderItem.ItemOrdered.ProductId);

                if (product is null)
                {
                    return Errors.Order.ProductNotFound(orderItem.ItemOrdered.ProductId);
                }

                product.UpdateStockQuantity(product.StockQuantity - orderItem.Quantity);
            }
        }

        return Result.Updated;
    }
}
