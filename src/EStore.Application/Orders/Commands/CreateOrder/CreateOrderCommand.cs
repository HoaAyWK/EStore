using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    CustomerId CustomerId,
    OrderStatus OrderStatus,
    string? TransactionId,
    ShippingAddress ShippingAddress)
    : IRequest<Order>;
