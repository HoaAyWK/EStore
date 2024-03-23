using ErrorOr;
using EStore.Application.Common.Dtos;
using EStore.Application.Common.Interfaces.Authentication;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Infrastructure.Authentication;
using EStore.Infrastructure.Common.EmailContents;
using EStore.Infrastructure.Common.Errors;
using EStore.Infrastructure.Identity;
using EStore.Infrastructure.Identity.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace EStore.Infrastructure.Services;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ICustomerRepository _customerRepository;
    private readonly IAccountTokenService _accountTokenService;
    private readonly IEmailService _emailService;
    private readonly IOtpService _otpService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        ICustomerRepository customerRepository,
        IAccountTokenService accountTokenService,
        IEmailService emailService,
        IOtpService otpService,
        IWebHostEnvironment webHostEnvironment,
        IDateTimeProvider dateTimeProvider)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _customerRepository = customerRepository;
        _accountTokenService = accountTokenService;
        _emailService = emailService;
        _otpService = otpService;
        _webHostEnvironment = webHostEnvironment;
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
            if (!appUser.EmailConfirmed)
            {
                return Errors.Authentication.NotConfirmedEmail;
            }

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

        var otp = _otpService.GenerateOtp();
        var accountToken = new AccountToken(
            email,
            otp,
            TokenType.ConfirmEmailOTP,
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

    public async Task<ErrorOr<Success>> VerifyEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return Errors.User.NotFound;
        }

        var accountToken = await _accountTokenService.GetAccountTokenAsync(token, email);

        if (accountToken is null)
        {
            return AccountTokenErrors.InvalidAccountToken;
        }

        if (accountToken.ExpireDate <= _dateTimeProvider.UtcNow)
        {
            return AccountTokenErrors.TokenExpired;
        }

        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);
        await _accountTokenService.DeleteTokenAsync(accountToken);

        return Result.Success;
    }
    
    public async Task<ErrorOr<Success>> ForgetPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return Errors.User.NotFound;
        }

        var otp = _otpService.GenerateOtp();
        var accountToken = new AccountToken(
            email,
            otp,
            TokenType.ForgotPasswordOtp,
            _dateTimeProvider.UtcNow.AddMinutes(10));

        await _accountTokenService.AddAccountTokenAsync(accountToken);

        var templatePath = GetTemplatePath();
        var htmlBody = await File.ReadAllTextAsync(templatePath);

        htmlBody = htmlBody
            .Replace("{0}", EmailContents.Auth.ForgetPasswordOTP)
            .Replace("{1}", otp);

        _ = Task.Run(() => _emailService.SendEmailWithTemplateAsync(
            subject: "[EStore] OTP for Email Confirmation",
            mailTo: email,
            htmlBody: htmlBody));

        return Result.Success;
    }

    public async Task<ErrorOr<Success>> ResetPasswordAsync(
        string email,
        string token,
        string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return Errors.User.NotFound;
        }

        var accountToken = await _accountTokenService.GetAccountTokenAsync(token, email);

        if (accountToken is null)
        {
            return AccountTokenErrors.InvalidAccountToken;
        }

        if (accountToken.ExpireDate <= _dateTimeProvider.UtcNow)
        {
            return AccountTokenErrors.TokenExpired;
        }

        var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        if (resetPasswordToken is null)
        {
            return Errors.General.UnProcessableRequest;
        }

        await _userManager.ResetPasswordAsync(user, resetPasswordToken, password);
        await _accountTokenService.DeleteTokenAsync(accountToken);

        return Result.Success;
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
