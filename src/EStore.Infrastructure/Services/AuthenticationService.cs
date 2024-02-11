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
using EStore.Infrastructure.Identity.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace EStore.Infrastructure.Services;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ICustomerRepository _customerRepository;
    private readonly IAccountTokenService _accountTokenService;
    private readonly IEmailService _emailService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        ICustomerRepository customerRepository,
        IAccountTokenService accountTokenService,
        IEmailService emailService,
        IDateTimeProvider dateTimeProvider)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _customerRepository = customerRepository;
        _accountTokenService = accountTokenService;
        _emailService = emailService;
        _dateTimeProvider = dateTimeProvider;
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

    public async Task<ErrorOr<Success>> SendConfirmationEmailAddressEmailAsync(string email, string templatePath)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var otp = GenerateOtp();
        var accountToken = new AccountToken(
            email,
            otp,
            TokenType.ForgotPasswordOtp,
            _dateTimeProvider.UtcNow.AddMinutes(10));

        await _accountTokenService.AddAccountTokenAsync(accountToken);

        var htmlBody = await File.ReadAllTextAsync(templatePath);

        htmlBody = htmlBody.Replace("{0}", otp);

        _ = Task.Run(() => _emailService.SendEmailWithTemplateAsync(
            subject: "[EStore] OTP for Email Confirmation",
            mailTo: email,
            htmlBody: htmlBody));
        
        return Result.Success;
    }

    private static string GenerateOtp(int length = 6)
    {
        var random = new Random();
        var opt = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            opt.Append(random.Next(0, 9));
        }

        return opt.ToString();
    }
}
