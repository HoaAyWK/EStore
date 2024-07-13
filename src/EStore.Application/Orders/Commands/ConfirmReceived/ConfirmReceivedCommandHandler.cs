using ErrorOr;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.Common.Errors;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.Repositories;
using MediatR;

namespace EStore.Application.Orders.Commands.ConfirmReceived;

public class ConfirmReceivedCommandHandler
    : IRequestHandler<ConfirmReceivedCommand, ErrorOr<Success>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ConfirmReceivedCommandHandler(
        IOrderRepository orderRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _orderRepository = orderRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<Success>> Handle(
        ConfirmReceivedCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return Errors.Order.NotFound;
        }

        if (order.OrderStatus != OrderStatus.Processing)
        {
             return Errors.Order.OrderStatusTransitionNotAllow(
                order.OrderStatus,
                OrderStatus.Completed);
        }

        var lastOrderStatusTracking = order.OrderStatusHistoryTrackings
            .OrderByDescending(x => x.CreatedDateTime)
            .FirstOrDefault();

        if (lastOrderStatusTracking is null)
        {
            return Errors.Order.OrderDidNotHaveAnyStatusTracking;
        }

        if (lastOrderStatusTracking.Status != OrderStatusHistory.OrderReceived)
        {
            return Errors.Order.OrderHasNotDeliveredYet;
        }

        order.AddOrderStatusHistoryTracking(
                OrderStatusHistory.OrderCompleted,
                _dateTimeProvider.UtcNow);

        order.UpdateOrderStatus(OrderStatus.Completed);
        order.UpdatePaymentStatus(PaymentStatus.Paid);

        return Result.Success;
    }
}
