using ErrorOr;
using EStore.Domain.CartAggregate;
using EStore.Domain.CartAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Carts.Commands.RemoveItemFromCart;

public class RemoveItemFromCartCommandHandler
    : IRequestHandler<RemoveItemFromCartCommand, ErrorOr<Cart>>
{
    private readonly ICartRepository _cartRepository;

    public RemoveItemFromCartCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<ErrorOr<Cart>> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.CartId);

        if (cart is null)
        {
            return Errors.Cart.NotFound;
        }

        cart.RemoveItem(request.ItemId);
        
        return cart;
    }
}
