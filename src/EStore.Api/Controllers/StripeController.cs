using Microsoft.AspNetCore.Mvc;
using EStore.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using MapsterMapper;
using EStore.Application.Orders.Commands.CreateCheckoutSession;
using Stripe;
using Stripe.Checkout;
using EStore.Infrastructure.Services.Settings;
using Microsoft.Extensions.Options;
using EStore.Domain.OrderAggregate.ValueObjects;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Application.Orders.Commands.UpdateOrder;
using EStore.Application.Orders.Commands.MarkOrderAsRefunded;

namespace EStore.Api.Controllers;

public class StripeController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly StripeSettings _stripeSettings;

    public StripeController(
        ISender mediator,
        IMapper mapper,
        IOptions<StripeSettings> stripeSettingsOptions)
    {
        _mediator = mediator;
        _mapper = mapper;
        _stripeSettings = stripeSettingsOptions.Value;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCheckoutSession()
    {
        Guid? id = GetCustomerId();

        if (id is null)
        {
            return Problem(new List<Error> { Errors.Authentication.Unauthenticated });
        }

        var command = _mapper.Map<CreateCheckoutSessionCommand>(id.Value);
        var createCheckoutSessionResult = await _mediator.Send(command);

        return createCheckoutSessionResult.Match(
            sessionUrl => Ok(sessionUrl),
            errors => Problem(errors));
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _stripeSettings.WebhookSecret
            );

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;

                Console.WriteLine("Catch CheckoutSessionCompleted");

                if (session is not null)
                {
                    string paymentIntentId = session.PaymentIntentId;
                    var shippingAddress = ShippingAddress.Create(
                        street: session.ShippingDetails.Address.Line1,
                        city: session.ShippingDetails.Address.City,
                        state: session.ShippingDetails.Address.State,
                        country: session.ShippingDetails.Address.State,
                        zipCode: session.ShippingDetails.Address.PostalCode);

                    var orderId = session.Metadata["order_id"];

                    if (orderId is not null)
                    {
                        var command = new UpdateOrderCommand(
                            OrderId.Create(new Guid(orderId)),
                            OrderStatus.Paid,
                            paymentIntentId,
                            shippingAddress);
                        
                        var updateOrderResult = await _mediator.Send(command);

                        // TODO: log errors when they occurred
                    }
                }
            }
            else if (stripeEvent.Type == Events.ChargeRefunded)
            {
                var charge = stripeEvent.Data.Object as Charge;

                Console.WriteLine("Catch charge refunded");
                
                if (charge is not null)
                {
                    var command = new MarkOrderAsRefundedCommand(charge.PaymentIntentId);

                    await _mediator.Send(command);
                }

            }

            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest(e.Message);
        }
    }

    private StatusCodeResult RedirectTo(string url)
    {
        Response.Headers.Add("Location", url);
        return new StatusCodeResult(303);
    }

    private Guid? GetCustomerId()
    {
        if (Request.HttpContext.User.Identity is not null)
        {
            if (Request.HttpContext.User.Identity.IsAuthenticated)
            {
                var id = Request.HttpContext.User.Identity.Name;

                if (Guid.TryParse(id, out Guid customerFromCtxId))
                {
                    return customerFromCtxId;
                }
            }
        }

        if (Request.Cookies.ContainsKey("Cart"))
        {
            var id = Request.Cookies["Cart"];

            if (Guid.TryParse(id, out Guid customerFromCookieId))
            {
                return customerFromCookieId;
            }
        }

        return null;
    }
}
