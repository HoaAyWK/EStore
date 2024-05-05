using ErrorOr;
using EStore.Contracts.Orders;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(OrderId OrderId)
    : IRequest<ErrorOr<OrderResponse>>;
