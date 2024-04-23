using ErrorOr;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using MediatR;

namespace EStore.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    CustomerId CustomerId,
    AddressId AddressId)
    : IRequest<ErrorOr<Order>>;
