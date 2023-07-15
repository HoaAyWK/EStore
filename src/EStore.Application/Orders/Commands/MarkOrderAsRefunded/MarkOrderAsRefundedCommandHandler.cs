using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.OrderAggregate.Repositories;
using MediatR;

namespace EStore.Application.Orders.Commands.MarkOrderAsRefunded;

public class MarkOrderAsRefundedCommandHandler
    : IRequestHandler<MarkOrderAsRefundedCommand, ErrorOr<Updated>>
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsRefundedCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(MarkOrderAsRefundedCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByTransactionIdAsync(request.TransactionId);

        if (order is null)
        {
            return Errors.Order.NotFound;
        }

        order.MarkAsRefunded();

        return Result.Updated;
    }
}
