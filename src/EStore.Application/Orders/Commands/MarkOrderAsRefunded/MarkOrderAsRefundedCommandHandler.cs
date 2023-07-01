using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Errors;
using EStore.Domain.OrderAggregate.Repositories;
using MediatR;

namespace EStore.Application.Orders.Commands.MarkOrderAsRefunded;

public class MarkOrderAsRefundedCommandHandler
    : IRequestHandler<MarkOrderAsRefundedCommand, ErrorOr<Updated>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkOrderAsRefundedCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(MarkOrderAsRefundedCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByTransactionIdAsync(request.TransactionId);

        if (order is null)
        {
            return Errors.Order.NotFound;
        }

        order.MarkAsRefunded();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
