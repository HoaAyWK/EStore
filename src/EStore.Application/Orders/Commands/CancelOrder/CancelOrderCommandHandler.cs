using ErrorOr;
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

    public CancelOrderCommandHandler(
        IOrderRepository orderRepository,
        IPaymentService paymentService)
    {
        _orderRepository = orderRepository;
        _paymentService = paymentService;
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

        if (order.PaymentMethod == PaymentMethod.CashOnDelivery ||
            order.PaymentStatus != PaymentStatus.Paid)
        {
            return Result.Success;
        }

        await _paymentService.ProcessRefundAsync(order, cancellationToken);

        return Result.Success;
    }
}
