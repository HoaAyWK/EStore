using EStore.Contracts.Common;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Enumerations;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrderListPaged;

public record GetOrderListPagedQuery(
    int Page,
    int PageSize,
    OrderStatus? OrderStatus,
    CustomerId CustomerId) : IRequest<PagedList<Order>>;
