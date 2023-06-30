using Microsoft.AspNetCore.Mvc;
using EStore.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using MapsterMapper;
using EStore.Application.Orders.Commands.CreateCheckoutSession;
using Stripe;
using EStore.Application.Orders.Commands.CreateOrder;
using Stripe.Checkout;

namespace EStore.Api.Controllers;

public class StripeController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public StripeController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
            sessionUrl => RedirectTo(sessionUrl),
            errors => Problem(errors));
    }

    private StatusCodeResult RedirectTo(string url)
    {
        Response.Headers.Add("Location", url);
        return new StatusCodeResult(303);
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ParseEvent(json);

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;

                Console.WriteLine("Catch CheckoutSessionCompleted");

                if (session is not null)
                {
                    
                }


            }
            else if (stripeEvent.Type == Events.PaymentIntentCanceled)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                Console.WriteLine("Catch payment intent canceled");
            }
            else if (stripeEvent.Type == Events.ChargeRefunded)
            {
                var charge = stripeEvent.Data.Object as Charge;

                Console.WriteLine("Catch charge refunded");
            }

            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest(e.Message);
        }
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
