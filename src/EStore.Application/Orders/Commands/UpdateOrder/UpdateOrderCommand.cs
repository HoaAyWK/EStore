using ErrorOr;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(
    OrderId OrderId,
    OrderStatus OrderStatus,
    string? TransactionId,
    ShippingAddress? ShippingAddress = null)
    : IRequest<ErrorOr<Updated>>;
