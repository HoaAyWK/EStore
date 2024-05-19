using EStore.Application.Orders.Services;
using EStore.Contracts.Orders;
using MediatR;

namespace EStore.Application.Orders.Queries.GetIncomeStats;

public class GetIncomeStatsQueryHandler
    : IRequestHandler<GetIncomeStatsQuery, IncomeStats>
{
    private readonly IOrderReadService _orderReadService;

    public GetIncomeStatsQueryHandler(IOrderReadService orderReadService)
    {
        _orderReadService = orderReadService;
    }

    public async Task<IncomeStats> Handle(
        GetIncomeStatsQuery request,
        CancellationToken cancellationToken)
    {
        return await _orderReadService.GetIncomeStatsAsync(request.LastDaysCount);
    }
}
