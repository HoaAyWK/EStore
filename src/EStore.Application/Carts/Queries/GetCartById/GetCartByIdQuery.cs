using ErrorOr;
using EStore.Contracts.Carts;
using EStore.Domain.CartAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Carts.Queries.GetCartById;

public record GetCartByIdQuery(CartId CartId)
    : IRequest<ErrorOr<CartResponse>>;
