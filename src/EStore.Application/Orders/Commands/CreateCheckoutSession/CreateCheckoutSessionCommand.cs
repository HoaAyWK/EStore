using ErrorOr;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Commands.CreateCheckoutSession;

public record CreateCheckoutSessionCommand(CustomerId CustomerId)
    : IRequest<ErrorOr<string>>;
