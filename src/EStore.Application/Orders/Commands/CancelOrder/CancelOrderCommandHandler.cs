using ErrorOr;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Orders.Services;
using EStore.Domain.Common.Errors;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.Repositories;
using MediatR;

namespace EStore.Application.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler
    : IRequestHandler<CancelOrderCommand, ErrorOr<Success>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentService _paymentService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CancelOrderCommandHandler(
        IOrderRepository orderRepository,
        IPaymentService paymentService,
        IDateTimeProvider dateTimeProvider)
    {
        _orderRepository = orderRepository;
        _paymentService = paymentService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<Success>> Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return Errors.Order.NotFound;
        }

        if (order.OrderStatus != OrderStatus.Pending)
        {
            return Errors.Order.CannotCancelOrder;
        }

        order.MarkAsCancelled();
        order.AddOrderStatusHistoryTracking(
            OrderStatusHistory.OrderCancelled,
            _dateTimeProvider.UtcNow);

        if (order.PaymentMethod == PaymentMethod.CashOnDelivery ||
            order.PaymentStatus != PaymentStatus.Paid)
        {
            return Result.Success;
        }

        await _paymentService.ProcessRefundAsync(order, cancellationToken);

        return Result.Success;
    }
}
