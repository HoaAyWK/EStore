using ErrorOr;
using EStore.Application.Carts.Services;
using EStore.Contracts.Carts;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Carts.Queries.GetCartByCustomerId;

public class GetCartByCustomerIdQueryHandler
    : IRequestHandler<GetCartByCustomerIdQuery, ErrorOr<CartResponse>>
{
    private readonly ICartReadService _cartReadService;

    public GetCartByCustomerIdQueryHandler(ICartReadService cartReadService)
    {
        _cartReadService = cartReadService;
    }

    public async Task<ErrorOr<CartResponse>> Handle(GetCartByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartReadService.GetByCustomerIdAsync(request.CustomerId);

        if (cart is null)
        {
            return Errors.Cart.NotFound;
        }

        return cart;
    }
}
