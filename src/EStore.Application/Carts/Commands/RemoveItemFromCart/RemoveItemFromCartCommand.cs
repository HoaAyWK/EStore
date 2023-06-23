using ErrorOr;
using EStore.Domain.CartAggregate;
using EStore.Domain.CartAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Carts.Commands.RemoveItemFromCart;

public record RemoveItemFromCartCommand(CartId CartId, CartItemId ItemId)
    : IRequest<ErrorOr<Cart>>;
