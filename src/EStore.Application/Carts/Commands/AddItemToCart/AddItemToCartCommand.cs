using ErrorOr;
using EStore.Domain.CartAggregate;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Carts.Commands.AddItemToCart;

public record AddItemToCartCommand(
    CustomerId CustomerId,
    ProductId ProductId,
    ProductVariantId? ProductVariantId = null,
    int Quantity = 1)
    : IRequest<ErrorOr<Cart>>;
