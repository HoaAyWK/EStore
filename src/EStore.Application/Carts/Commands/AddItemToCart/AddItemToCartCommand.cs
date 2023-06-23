using ErrorOr;
using EStore.Domain.CartAggregate;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Carts.Commands.AddItemToCart;

public record AddItemToCartCommand(
    CustomerId CustomerId,
    ProductId ProductId,
    ProductVariantId? ProductVariantId = null)
    : IRequest<ErrorOr<Cart>>;
