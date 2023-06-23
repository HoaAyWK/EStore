using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.CartAggregate;
using EStore.Domain.CartAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Carts.Commands.RemoveItemFromCart;

public class RemoveItemFromCartCommandHandler
    : IRequestHandler<RemoveItemFromCartCommand, ErrorOr<Cart>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveItemFromCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Cart>> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.CartId);

        if (cart is null)
        {
            return Errors.Cart.NotFound;
        }

        cart.RemoveItem(request.ItemId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return cart;
    }
}
