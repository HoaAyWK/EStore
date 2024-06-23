using ErrorOr;
using EStore.Application.Carts.Services;
using EStore.Contracts.Carts;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Carts.Queries.GetCartById;

public class GetCartByIdQueryHandler
    : IRequestHandler<GetCartByIdQuery, ErrorOr<CartResponse>>
{
    private readonly ICartReadService _cartReadService;

    public GetCartByIdQueryHandler(ICartReadService cartReadService)
    {
        _cartReadService = cartReadService;
    }

    public async Task<ErrorOr<CartResponse>> Handle(
        GetCartByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cart = await _cartReadService.GetByIdAsync(request.CartId);

        if (cart is null)
        {
            return Errors.Cart.NotFound;
        }

        return cart;
    }
}
