using System.Text;
using ErrorOr;
using EStore.Application.Common.Dtos;
using EStore.Application.Common.Interfaces.Authentication;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Infrastructure.Authentication;
using EStore.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace EStore.Infrastructure.Services;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmailService _emailService;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenGenerator jwtTokenGenerator,
        ICustomerRepository customerRepository,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _customerRepository = customerRepository;
        _emailService = emailService;
    }

    public async Task<ErrorOr<AuthenticationResult>> LoginAsync(string email, string password)
    {
        var customer = await _customerRepository.GetByEmailAsync(email);

        if (customer is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var appUser = await _userManager.FindByEmailAsync(email);

        if (appUser is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var validPassword = await _userManager.CheckPasswordAsync(appUser, password);

        if (validPassword)
        {
            var generateTokenResult = await _jwtTokenGenerator.GenerateTokenAsync(customer.Id.Value.ToString());

            if (generateTokenResult.IsError)
            {
                return generateTokenResult.Errors;
            }

            return new AuthenticationResult()
            {
                Customer = customer,
                Token = generateTokenResult.Value
            };
        }

        return Errors.Authentication.InvalidCredentials;
    }

    public async Task<ErrorOr<AuthenticationResult>> RegisterAsync(Customer customer, string password)
    {
        var customerId = customer.Id.Value.ToString();

        var appUser = new ApplicationUser
        {
            Id = customerId,
            Email = customer.Email,
            UserName = customer.Email,
            EmailConfirmed = true
        };

        var createAppUserResult = await _userManager.CreateAsync(appUser, password);

        if (createAppUserResult.Succeeded)
        {
            await _userManager.AddToRoleAsync(appUser, Roles.Customer);
            var generateTokenResult = await _jwtTokenGenerator.GenerateTokenAsync(customerId);

            if (generateTokenResult.IsError)
            {
                return generateTokenResult.Errors;
            }

            return new AuthenticationResult()
            {
                Customer = customer,
                Token = generateTokenResult.Value
            };
        }

        List<Error> errors = new List<Error>();

        foreach (var error in createAppUserResult.Errors)
        {
            errors.Add(Error.Validation(
                code: error.Code,
                description: error.Description));
        }

        return errors;
    }

    public async Task<ErrorOr<Success>> SendConfirmationEmailAddressEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

        await _emailService.SendEmailAsync(
            subject: "Confirm Your Email Address",
            mailTo: email,
            body: encodedToken);
        
        return Result.Success;
    }
}
