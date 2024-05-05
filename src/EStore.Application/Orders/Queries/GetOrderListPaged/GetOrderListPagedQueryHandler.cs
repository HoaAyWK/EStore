using EStore.Application.Orders.Services;
using EStore.Contracts.Common;
using EStore.Contracts.Orders;
using EStore.Domain.OrderAggregate.Enumerations;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrderListPaged;

public class GetOrderListPagedQueryHandler
    : IRequestHandler<GetOrderListPagedQuery, PagedList<OrderResponse>>
{
    private readonly IOrderReadService _orderReadService;

    public GetOrderListPagedQueryHandler(IOrderReadService orderReadService)
    {
        _orderReadService = orderReadService;
    }

    public async Task<PagedList<OrderResponse>> Handle(
        GetOrderListPagedQuery request,
        CancellationToken cancellationToken)
    {
        return await _orderReadService.GetListPagedAsync(
            request.Page,
            request.PageSize,
            request.OrderStatus,
            request.Order,
            request.OrderBy,
            request.OrderNumber);
    }
}
