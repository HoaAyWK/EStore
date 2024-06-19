using EStore.Application.Orders.Services;
using EStore.Contracts.Common;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Enumerations;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerQueryHandler
    : IRequestHandler<GetOrdersByCustomerQuery, PagedList<Order>>
{
    private readonly IOrderReadService _orderReadService;

    public GetOrdersByCustomerQueryHandler(IOrderReadService orderReadService)
    {
        _orderReadService = orderReadService;
    }

    public async Task<PagedList<Order>> Handle(
        GetOrdersByCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var orderStatus = string.IsNullOrWhiteSpace(request.OrderStatus)
            ? null
            : OrderStatus.FromName(request.OrderStatus);

        return await _orderReadService.GetByCustomerIdAsync(
            request.CustomerId,
            request.Page,
            request.PageSize,
            orderStatus,
            request.Order,
            request.OrderBy,
            cancellationToken);
    }
}
