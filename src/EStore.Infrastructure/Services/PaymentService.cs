using EStore.Application.Orders.Services;
using EStore.Contracts.Carts;
using EStore.Infrastructure.Services.Settings;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace EStore.Infrastructure.Services;

internal sealed class PaymentService : IPaymentService
{
    private readonly StripeSettings _stripeSettings;

    public PaymentService(IOptions<StripeSettings> options)
    {
        _stripeSettings = options.Value;
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }

    public async Task<string> ProcessPaymentAsync(CartResponse cart)
    {
        var lineItems = new List<SessionLineItemOptions>();

        foreach (var cartItem in cart.Items)
        {
            string productName = $"{cartItem.ProductName}";
            if (!string.IsNullOrEmpty(cartItem.ProductAttributes))
            {
                productName += $"({cartItem.ProductAttributes})";
            }

            var lineItem = new SessionLineItemOptions
            {
                Quantity = cartItem.Quantity,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = productName
                    },
                    UnitAmount = (long)cartItem.ProductPrice * 100,
                    Currency = "usd"
                },
            };

            lineItems.Add(lineItem);
        }

        var options = new SessionCreateOptions
        {
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = _stripeSettings.SuccessUrl,
            CancelUrl = _stripeSettings.CancelUrl
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        return session.Url;
    }
} 
