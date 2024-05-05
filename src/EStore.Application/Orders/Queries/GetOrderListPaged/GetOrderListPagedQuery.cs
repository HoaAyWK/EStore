using EStore.Contracts.Common;
using EStore.Contracts.Orders;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrderListPaged;

public record GetOrderListPagedQuery(
    int Page,
    int PageSize,
    string? OrderStatus,
    string? Order,
    string? OrderBy,
    int? OrderNumber) : IRequest<PagedList<OrderResponse>>;
