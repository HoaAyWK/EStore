using EStore.Application.Carts.Services;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.CartAggregate;
using EStore.Domain.CartAggregate.Repositories;
using EStore.Domain.CustomerAggregate.ValueObjects;

namespace EStore.Infrastructure.Services;

internal sealed class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CartService(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task TransferCartAsync(Guid anonymousId, Guid customerId)
    {
        if (anonymousId == customerId) return;

        var anonymousCart = await _cartRepository
            .GetCartByCustomerId(CustomerId.Create(anonymousId));

        if (anonymousCart is null) return;

        var customerCart = await _cartRepository
            .GetCartByCustomerId(CustomerId.Create(customerId));

        if (customerCart is null)
        {
            customerCart = Cart.Create(CustomerId.Create(customerId));
        }

        foreach (var anonymousCartItem in anonymousCart.Items)
        {
            customerCart.AddItem(
                anonymousCartItem.ProductId,
                anonymousCartItem.ProductVariantId,
                anonymousCartItem.UnitPrice,
                anonymousCartItem.Quantity);
        }

        _cartRepository.Delete(anonymousCart);
        await _unitOfWork.SaveChangesAsync();
    }
}
