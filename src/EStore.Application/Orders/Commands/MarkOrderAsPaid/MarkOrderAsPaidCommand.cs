using ErrorOr;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Commands.MarkOrderAsPaid;

public record MarkOrderAsPaidCommand(OrderId OrderId) : IRequest<ErrorOr<Updated>>;
