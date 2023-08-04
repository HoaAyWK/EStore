using ErrorOr;
using EStore.Domain.CartAggregate;
using EStore.Domain.CartAggregate.Repositories;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Repositories;
using MediatR;

namespace EStore.Application.Carts.Commands.AddItemToCart;

public class AddItemToCartCommandHandler
    : IRequestHandler<AddItemToCartCommand, ErrorOr<Cart>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public AddItemToCartCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<Cart>> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Cart.ProductNotFound(request.ProductId);
        }

        if (!product.Published)
        {
            return Errors.Cart.ProductIsNotPublished(request.ProductId);
        }

        var itemPrice = product.Price;

        if (product.HasVariant && request.ProductVariantId is null)
        {
            return Errors.Cart.InvalidProductVariant;
        }

        if (request.ProductVariantId is not null)
        {
            var productVariant = product.ProductVariants.FirstOrDefault(v => v.Id == request.ProductVariantId);

            if (productVariant is null)
            {
                return Errors.Cart.ProductVariantNotFound(request.ProductVariantId);
            }

            if (!productVariant.IsActive)
            {
                return Errors.Cart.ProductVariantUnavailable(productVariant.Id);
            }

            if (!product.ProductVariants.Any(variant => variant.Id == productVariant.Id))
            {
                return Errors.Cart.ProductNotHadVariant(request.ProductId, request.ProductVariantId);
            }

            if (productVariant.Price.HasValue)
            {
                itemPrice += productVariant.Price.Value;
            }
        }
        

        var cart = await _cartRepository.GetCartByCustomerId(request.CustomerId);

        if (cart is null)
        {
            cart = Cart.Create(request.CustomerId);
            await _cartRepository.AddAsync(cart);
        }

        var addItemResult = cart.AddItem(request.ProductId, request.ProductVariantId, itemPrice);

        if (addItemResult.IsError)
        {
            return addItemResult.Errors;
        }

        return cart;
    }
}
