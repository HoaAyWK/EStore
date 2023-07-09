using Microsoft.AspNetCore.Mvc;
using EStore.Contracts.Authentication;
using MediatR;
using MapsterMapper;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Customers.Commands.CreateCustomer;
using EStore.Application.Carts.Services;

namespace EStore.Api.Controllers;

public class AuthController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;
    private readonly ICartService _cartService;

    public AuthController(
        ISender mediator,
        IMapper mapper,
        IAuthenticationService authenticationService,
        ICartService cartService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _authenticationService = authenticationService;
        _cartService = cartService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<CreateCustomerCommand>(request);
        var createUserResult = await _mediator.Send(command);

        if (createUserResult.IsError)
        {
            return Problem(createUserResult.Errors);
        }

        var user = createUserResult.Value;
        var authResult = await _authenticationService.RegisterAsync(user, request.Password);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var authResult = await _authenticationService.LoginAsync(request.Email, request.Password);

        if (!authResult.IsError)
        {

            await TransferAnonymousCartToCustomerCartAsync(authResult.Value.Customer.Id.Value);
        }

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }

    [HttpPost("send-confirmation-email")]
    public async Task<IActionResult> SendConfirmationEmail(SendConfirmationEmailRequest request)
    {
        var sendEmailResult = await _authenticationService.SendConfirmationEmailAddressEmailAsync(request.Email);

        return sendEmailResult.Match(
            success => NoContent(),
            errors => Problem(errors));
    }


    private async Task TransferAnonymousCartToCustomerCartAsync(Guid customerId)
    {
        if (Request.Cookies.ContainsKey("Cart"))
        {
            var anonymousId = Request.Cookies["Cart"];

            if (Guid.TryParse(anonymousId, out Guid _))
            {
                await _cartService.TransferCartAsync(new Guid(anonymousId), customerId);
            }

            Response.Cookies.Delete("Cart");
        }
    }
}
