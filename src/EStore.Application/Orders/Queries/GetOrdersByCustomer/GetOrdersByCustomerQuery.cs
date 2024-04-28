using EStore.Contracts.Common;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrdersByCustomer;

public record GetOrdersByCustomerQuery(
    CustomerId CustomerId,
    int Page,
    int PageSize,
    string? OrderStatus = null)
    : IRequest<PagedList<Order>>;
