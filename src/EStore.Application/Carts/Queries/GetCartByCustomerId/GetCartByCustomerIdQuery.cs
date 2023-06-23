using ErrorOr;
using EStore.Contracts.Carts;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Carts.Queries.GetCartByCustomerId;

public record GetCartByCustomerIdQuery(CustomerId CustomerId)
    : IRequest<ErrorOr<CartResponse>>;
