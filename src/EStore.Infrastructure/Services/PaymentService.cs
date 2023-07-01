using EStore.Application.Orders.Services;
using EStore.Domain.OrderAggregate;
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

    public async Task<string> ProcessPaymentAsync(Order order)
    {
        var lineItems = new List<SessionLineItemOptions>();

        foreach (var orderItem in order.OrderItems)
        {
            string productName = $"{orderItem.ItemOrdered.ProductName}";
            if (!string.IsNullOrEmpty(orderItem.ItemOrdered.ProductAttributes))
            {
                productName += $" ({orderItem.ItemOrdered.ProductAttributes})";
            }

            var lineItem = new SessionLineItemOptions
            {
                Quantity = orderItem.Quantity,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = productName
                    },
                    UnitAmount = (long)orderItem.UnitPrice * 100,
                    Currency = "usd"
                },
            };

            lineItems.Add(lineItem);
        }

        var options = new SessionCreateOptions
        {
            LineItems = lineItems,
            ShippingAddressCollection = new SessionShippingAddressCollectionOptions
            {
                AllowedCountries = new List<string> { "VN" }
            },
            Mode = "payment",
            SuccessUrl = _stripeSettings.SuccessUrl,
            CancelUrl = _stripeSettings.CancelUrl,
            Metadata = new Dictionary<string, string>()
            {
                { "order_id", order.Id.Value.ToString() }
            }
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        return session.Url;
    }

    public async Task ProcessRefundAsync(Order order)
    {
        var options = new RefundCreateOptions { PaymentIntent = order.TransactionId };
        var service = new RefundService();

        await service.CreateAsync(options);
    }
} 
