using EStore.Contracts.Common;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrdersByCustomer;

public record GetOrdersByCustomerQuery(
    CustomerId CustomerId,
    int Page,
    int PageSize,
    string? OrderStatus = null,
    string? Order = null,
    string? OrderBy = null)
    : IRequest<PagedList<Order>>;
