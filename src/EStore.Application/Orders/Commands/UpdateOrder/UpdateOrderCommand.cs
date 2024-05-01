using ErrorOr;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(
    OrderId OrderId,
    OrderStatus OrderStatus,
    PaymentStatus PaymentStatus,
    string? TransactionId)
    : IRequest<ErrorOr<Updated>>;
