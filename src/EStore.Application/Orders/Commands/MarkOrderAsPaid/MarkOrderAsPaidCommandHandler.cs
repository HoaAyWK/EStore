using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.Repositories;
using MediatR;

namespace EStore.Application.Orders.Commands.MarkOrderAsPaid;

public class MarkOrderAsPaidCommandHandler
    : IRequestHandler<MarkOrderAsPaidCommand, ErrorOr<Updated>>
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsPaidCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return Errors.Order.NotFound;
        }

        order.UpdateOrderStatus(OrderStatus.Paid);
        
        return Result.Updated;
    }
}
