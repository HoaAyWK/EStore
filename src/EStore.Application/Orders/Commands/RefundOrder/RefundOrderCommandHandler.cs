using ErrorOr;
using EStore.Application.Orders.Services;
using EStore.Domain.Common.Errors;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.Repositories;
using MediatR;

namespace EStore.Application.Orders.Commands.RefundOrder;

public class RefundOrderCommandHandler
    : IRequestHandler<RefundOrderCommand, ErrorOr<Success>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentService _paymentService;

    public RefundOrderCommandHandler(IOrderRepository orderRepository, IPaymentService paymentService)
    {
        _orderRepository = orderRepository;
        _paymentService = paymentService;
    }

    public async Task<ErrorOr<Success>> Handle(RefundOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return Errors.Order.NotFound;
        }

        if (order.OrderStatus != OrderStatus.Paid)
        {
            return Errors.Order.UnpaidOrder(order.Id);
        }

        if (order.TransactionId is not null)
        {
            await _paymentService.ProcessRefundAsync(order);
        }

        return Result.Success;
    }
}
