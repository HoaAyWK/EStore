using ErrorOr;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Commands.CancelOrder;

public record CancelOrderCommand(OrderId OrderId)
    : IRequest<ErrorOr<Success>>;
