using ErrorOr;
using EStore.Domain.CartAggregate;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Carts.Commands.CreateCart;

public record CreateCartCommand(CustomerId CustomerId)
    : IRequest<ErrorOr<Cart>>;
