using Microsoft.AspNetCore.Mvc;
using EStore.Contracts.Authentication;
using MediatR;
using MapsterMapper;
using EStore.Api.Common.ApiRoutes;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Customers.Commands.CreateCustomer;
using EStore.Application.Carts.Services;
using EStore.Api.Common;
using EStore.Api.Common.Contexts;

namespace EStore.Api.Controllers;

public class AuthController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;
    private readonly ICartService _cartService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IWorkContextSource _workContextSource;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ISender mediator,
        IMapper mapper,
        IAuthenticationService authenticationService,
        ICartService cartService,
        IWebHostEnvironment webHostEnvironment,
        IWorkContextSource workContextSource,
        ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _authenticationService = authenticationService;
        _cartService = cartService;
        _webHostEnvironment = webHostEnvironment;
        _workContextSource = workContextSource;
        _logger = logger;
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
            Problem);
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
            Problem);
    }

    [HttpPost(ApiRoutes.Auth.SendConfirmationEmail)]
    public async Task<IActionResult> SendConfirmationEmail(SendConfirmationEmailRequest request)
    {
        var templatePath = GetTemplatePath();
        var sendEmailResult = await _authenticationService
            .SendConfirmationEmailAddressEmailAsync(request.Email, templatePath);

        return sendEmailResult.Match(success => NoContent(), Problem);
    }

    [HttpPost(ApiRoutes.Auth.VerifyEmail)]
    public async Task<IActionResult> VerifyEmail(VerifyEmailRequest request)
    {
        var verifyEmailResult = await _authenticationService
            .VerifyEmailAsync(request.Email, request.Token);

        return verifyEmailResult.Match(success => NoContent(), Problem);
    }

    [HttpPost(ApiRoutes.Auth.ForgetPassword)]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
    {
        var forgetPasswordResult = await _authenticationService.ForgetPasswordAsync(request.Email);

        return forgetPasswordResult.Match(
            success => NoContent(),
            Problem);
    }

    [HttpPost(ApiRoutes.Auth.ResetPassword)]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var resetPasswordResult = await _authenticationService.ResetPasswordAsync(
            request.Email,
            request.Token,
            request.Password);

        return resetPasswordResult.Match(
            success => NoContent(),
            Problem);
    }

    [HttpGet(ApiRoutes.Auth.Logout)]
    public async Task<IActionResult> Logout()
    {
        _workContextSource.RemoveCookies(Constants.Cookies.Guest);

        await Task.CompletedTask;

        return NoContent();
    }

    private async Task TransferAnonymousCartToCustomerCartAsync(Guid customerId)
    {
        _logger.LogInformation("Start transferring anonymous cart to customer cart.");
        _logger.LogInformation("{@RequestCookies}", Request.Cookies);

        if (Request.Cookies.ContainsKey(Constants.Cookies.Guest))
        {
            var anonymousId = Request.Cookies[Constants.Cookies.Guest];

            if (Guid.TryParse(anonymousId, out Guid _))
            {
                _logger.LogInformation("Transferring cart from {AnonymousId} to {CustomerId}", anonymousId, customerId);
                
                await _cartService.TransferCartAsync(new Guid(anonymousId), customerId);
            }

            // Remove guest cookies
            _workContextSource.RemoveCookies(Constants.Cookies.Guest);
            _logger.LogInformation("Removed guest cookies {AnonymousId}", anonymousId);

            //  Add customer cookies
            _workContextSource.AppendCookies(Constants.Cookies.Guest, customerId);
            _logger.LogInformation("Added customer cookies {CustomerId}", customerId);
        }
        else
        {
            _logger.LogInformation("Transferring cart failed. Failed to read guest cookies.");
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
