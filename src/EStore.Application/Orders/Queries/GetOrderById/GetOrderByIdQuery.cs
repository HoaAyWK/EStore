using ErrorOr;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(OrderId OrderId)
    : IRequest<ErrorOr<Order>>;
