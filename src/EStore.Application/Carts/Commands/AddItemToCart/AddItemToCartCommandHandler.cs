using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.CartAggregate;
using EStore.Domain.CartAggregate.Repositories;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductVariantAggregate.Repositories;
using MediatR;

namespace EStore.Application.Carts.Commands.AddItemToCart;

public class AddItemToCartCommandHandler
    : IRequestHandler<AddItemToCartCommand, ErrorOr<Cart>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductVariantRepository _productVariantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddItemToCartCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IProductVariantRepository productVariantRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _productVariantRepository = productVariantRepository;
        _unitOfWork = unitOfWork;
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

        var productVariants = await _productVariantRepository.GetByProductIdAsync(request.ProductId);

        if (request.ProductVariantId is not null)
        {
            var productVariant = await _productVariantRepository.GetByIdAsync(request.ProductVariantId);

            if (productVariant is null)
            {
                return Errors.Cart.ProductVariantNotFound(request.ProductVariantId);
            }

            if (!productVariant.IsActive)
            {
                return Errors.Cart.ProductVariantUnavailable(productVariant.Id);
            }

            if (!productVariants.Any(variant => variant.Id == productVariant.Id))
            {
                return Errors.Cart.ProductNotHadVariant(request.ProductId, request.ProductVariantId);
            }

            if (productVariant.Price.HasValue)
            {
                itemPrice += productVariant.Price.Value;
            }
        }
        else
        {
            if (productVariants.Any())
            {
                return Errors.Cart.InvalidProductVariant;
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

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return cart;
    }
}
