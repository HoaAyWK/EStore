using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Errors;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.Repositories;
using MediatR;

namespace EStore.Application.Orders.Commands.MarkOrderAsPaid;

public class MarkOrderAsPaidCommandHandler
    : IRequestHandler<MarkOrderAsPaidCommand, ErrorOr<Updated>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkOrderAsPaidCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return Errors.Order.NotFound;
        }

        order.UpdateOrderStatus(OrderStatus.Paid);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
