using ErrorOr;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Commands.MarkOrderAsCompleted;

public record MarkOrderAsCompletedCommand(OrderId OrderId) : IRequest<ErrorOr<Updated>>;
