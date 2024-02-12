using Microsoft.AspNetCore.Mvc;
using EStore.Contracts.Authentication;
using MediatR;
using MapsterMapper;
using EStore.Api.Common.ApiRoutes;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Customers.Commands.CreateCustomer;
using EStore.Application.Carts.Services;
using ErrorOr;

namespace EStore.Api.Controllers;

public class AuthController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;
    private readonly ICartService _cartService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AuthController(
        ISender mediator,
        IMapper mapper,
        IAuthenticationService authenticationService,
        ICartService cartService,
        IWebHostEnvironment webHostEnvironment)
    {
        _mediator = mediator;
        _mapper = mapper;
        _authenticationService = authenticationService;
        _cartService = cartService;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpPost(ApiRoutes.Auth.Register)]
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

    [HttpPost(ApiRoutes.Auth.Login)]
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

    [HttpPost(ApiRoutes.Auth.SendConfirmationEmail)]
    public async Task<IActionResult> SendConfirmationEmail(SendConfirmationEmailRequest request)
    {
        var templatePath = GetTemplatePath();
        var sendEmailResult = await _authenticationService
            .SendConfirmationEmailAddressEmailAsync(request.Email, templatePath);

        return sendEmailResult.Match(
            success => NoContent(),
            errors => Problem(errors));
    }

    [HttpPost(ApiRoutes.Auth.VerifyEmail)]
    public async Task<IActionResult> VerifyEmail(VerifyEmailRequest request)
    {
        var verifyEmailResult = await _authenticationService
            .VerifyEmailAsync(request.Email, request.Token);

        return verifyEmailResult.Match(
            Success => NoContent(),
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

    private string GetTemplatePath()
    {
        var separator = Path.DirectorySeparatorChar.ToString();

        return _webHostEnvironment.WebRootPath
            + separator
            + "Templates"
            + separator
            + "otp-email.html";
    }
}
