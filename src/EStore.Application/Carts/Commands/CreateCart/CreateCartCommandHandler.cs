using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.CartAggregate;
using EStore.Domain.CartAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Carts.Commands.CreateCart;

public class CreateCartCommandHandler
    : IRequestHandler<CreateCartCommand, ErrorOr<Cart>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Cart>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartByCustomerId(request.CustomerId);

        if (cart is not null)
        {
            return Errors.Cart.Existing(request.CustomerId);
        }

        cart = Cart.Create(request.CustomerId);
        await _cartRepository.AddAsync(cart);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return cart;
    }
}
