using ErrorOr;
using EStore.Application.Carts.Services;
using EStore.Application.Orders.Services;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Orders.Commands.CreateCheckoutSession;

public class CreateCheckoutSessionCommandHandler
    : IRequestHandler<CreateCheckoutSessionCommand, ErrorOr<string>>
{
    private readonly IPaymentService _paymentService;
    private readonly ICartReadService _cartReadService;

    public CreateCheckoutSessionCommandHandler(
        IPaymentService paymentService,
        ICartReadService cartReadService)
    {
        _paymentService = paymentService;
        _cartReadService = cartReadService;
    }

    public async Task<ErrorOr<string>> Handle(
        CreateCheckoutSessionCommand request,
        CancellationToken cancellationToken)
    {
        var cart = await _cartReadService.GetByCustomerIdAsync(request.CustomerId);

        if (cart is null)
        {
            return Errors.Order.CartNotFound;
        }

        if (cart.Items.Count == 0)
        {
            return Errors.Order.CartIsEmpty;
        }

        string sessionUrl = await _paymentService.ProcessPaymentAsync(cart);

        return sessionUrl;
    }
}
