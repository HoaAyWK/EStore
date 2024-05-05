using ErrorOr;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Commands.ConfirmReceived;

public record ConfirmReceivedCommand(OrderId OrderId)
    : IRequest<ErrorOr<Success>>;
