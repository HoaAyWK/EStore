using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.Common.Errors;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.Events;
using EStore.Domain.OrderAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using MediatR;

namespace EStore.Application.Orders.Commands.ConfirmPaymentInfo;

public class ConfirmPaymentInfoCommandHandler
    : IRequestHandler<ConfirmPaymentInfoCommand, ErrorOr<Success>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public ConfirmPaymentInfoCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork,
        IPublisher publisher)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<ErrorOr<Success>> Handle(
        ConfirmPaymentInfoCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return Errors.Order.NotFound;
        }

        if (order.OrderStatus != OrderStatus.Pending)
        {
            return Errors.Order.OrderStatusTransitionNotAllow(
                order.OrderStatus,
                OrderStatus.Processing);
        }

        var validateOrderItemsResult = await ValidateOrderItemsAsync(order);

        if (validateOrderItemsResult.Count > 0)
        {
            return validateOrderItemsResult;
        }

        order.UpdateOrderStatus(OrderStatus.Processing);
        order.AddOrderStatusHistoryTracking(
            status: OrderStatusHistory.PaymentInfoConfirmed,
            createdDateTime: _dateTimeProvider.UtcNow);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _publisher.Publish(
            new PaymentInfoConfirmedDomainEvent(order.Id),
            cancellationToken: cancellationToken);

        return Result.Success;
    }

    private async Task<List<Error>> ValidateOrderItemsAsync(Order order)
    {
        var errors = new List<Error>();

        foreach (var orderItem in order.OrderItems)
        {
            var product = await _productRepository.GetByIdAsync(
                orderItem.ItemOrdered.ProductId);

            if (product is null)
            {
                errors.Add(Errors.Product.NotFoundValidation(
                    orderItem.ItemOrdered.ProductId.Value));

                continue;
            }

            if (!product.HasVariant)
            {
                var remainingStockQuantity = product.StockQuantity - orderItem.Quantity;

                if (remainingStockQuantity < 0)
                {
                    errors.Add(Errors.Product.NotEnoughStock(product.Id.Value));
                    continue;
                }

                continue;
            }

            if (orderItem.ItemOrdered.ProductVariantId is null)
            {
                continue;
            }

            var productVariant = product.ProductVariants
                .Where(variant => variant.Id == orderItem.ItemOrdered.ProductVariantId)
                .FirstOrDefault();

            if (productVariant is null)
            {
                errors.Add(Errors.Product.ProductVariantNotFoundValidation(
                    orderItem.ItemOrdered.ProductVariantId.Value));

                continue;
            }

            var remainingProductVariantStockQuantity = productVariant.StockQuantity - orderItem.Quantity;

            if (remainingProductVariantStockQuantity < 0)
            {
                errors.Add(Errors.Product.ProductVariantNotEnoughStock(productVariant.Id.Value));
            }
        }

        return errors;
    }
}


