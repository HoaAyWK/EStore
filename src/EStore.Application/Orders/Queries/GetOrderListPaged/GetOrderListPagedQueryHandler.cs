using EStore.Application.Orders.Services;
using EStore.Contracts.Common;
using EStore.Domain.OrderAggregate;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrderListPaged;

public class GetOrderListPagedQueryHandler
    : IRequestHandler<GetOrderListPagedQuery, PagedList<Order>>
{
    private readonly IOrderReadService _orderReadService;

    public GetOrderListPagedQueryHandler(IOrderReadService orderReadService)
    {
        _orderReadService = orderReadService;
    }

    public async Task<PagedList<Order>> Handle(
        GetOrderListPagedQuery request,
        CancellationToken cancellationToken)
    {
        return await _orderReadService.GetListPagedAsync(
            request.Page,
            request.PageSize);
    }
}
