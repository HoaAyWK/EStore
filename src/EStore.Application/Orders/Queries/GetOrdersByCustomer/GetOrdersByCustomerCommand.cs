using ErrorOr;
using EStore.Contracts.Common;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrdersByCustomer;

public record GetOrdersByCustomerCommand(CustomerId CustomerId, int Page, int PageSize)
    : IRequest<PagedList<Order>>;
