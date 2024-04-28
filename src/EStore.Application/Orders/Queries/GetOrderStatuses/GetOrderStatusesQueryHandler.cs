using EStore.Domain.OrderAggregate.Enumerations;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrderStatuses;

public class GetOrderStatusesQueryHandler : IRequestHandler<GetOrderStatusesQuery, List<OrderStatus>>
{
    public Task<List<OrderStatus>> Handle(GetOrderStatusesQuery request, CancellationToken cancellationToken)
    {
        var orderStatuses = OrderStatus.GetValues();

        return Task.FromResult(orderStatuses.ToList());
    }
}
