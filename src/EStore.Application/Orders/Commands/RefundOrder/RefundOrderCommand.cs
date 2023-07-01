using ErrorOr;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Commands.RefundOrder;

public record RefundOrderCommand(OrderId OrderId) : IRequest<ErrorOr<Success>>;
