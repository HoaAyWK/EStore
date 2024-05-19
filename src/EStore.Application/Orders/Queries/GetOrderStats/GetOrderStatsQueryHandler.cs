using EStore.Application.Orders.Services;
using EStore.Contracts.Orders;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrderStats;

public class GetOrderStatsQueryHandler
    : IRequestHandler<GetOrderStatsQuery, OrderStats>
{
    private readonly IOrderReadService orderReadService;

    public GetOrderStatsQueryHandler(IOrderReadService orderReadService)
    {
        this.orderReadService = orderReadService;
    }

    public async Task<OrderStats> Handle(
        GetOrderStatsQuery request,
        CancellationToken cancellationToken)
    {
        return await orderReadService.GetOrderStatsAsync(request.LastDaysCount);
    }
}
