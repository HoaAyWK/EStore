using System.Text;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Customers.Events.CustomerCreated;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Infrastructure.Common.EmailContents;
using EStore.Infrastructure.Identity;
using EStore.Infrastructure.Identity.Enums;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace EStore.Infrastructure.IntegrationEvents.Customers.CustomerCreated;

public sealed class CustomerCreatedIntegrationEventHandler : INotificationHandler<CustomerCreatedIntegrationEvent>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthenticationService _authenticationService;
    private readonly IAccountTokenService _accountTokenService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IOtpService _otpService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEmailService _emailService;

    public CustomerCreatedIntegrationEventHandler(
        ICustomerRepository customerRepository,
        UserManager<ApplicationUser> userManager,
        IAuthenticationService authenticationService,
        IAccountTokenService accountTokenService,
        IWebHostEnvironment webHostEnvironment,
        IOtpService otpService,
        IDateTimeProvider dateTimeProvider,
        IEmailService emailService)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
        _authenticationService = authenticationService;
        _accountTokenService = accountTokenService;
        _webHostEnvironment = webHostEnvironment;
        _otpService = otpService;
        _dateTimeProvider = dateTimeProvider;
        _emailService = emailService;
    }

    public async Task Handle(CustomerCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var user = await _customerRepository.GetByIdAsync(notification.CustomerId);

        if (user is null)
        {
            return;
        }

        var appUser = await _userManager.FindByIdAsync(user.Id.Value.ToString());

        if (appUser is null)
        {
            return;
        }

        var templatePath = GetTemplatePath();
        var otp = _otpService.GenerateOtp();

        var accountToken = new AccountToken(
            user.Email,
            otp,
            TokenType.ForgotPasswordOtp,
            _dateTimeProvider.UtcNow.AddMinutes(10));

        await _accountTokenService.AddAccountTokenAsync(accountToken);

        var htmlBody = await File.ReadAllTextAsync(templatePath, cancellationToken);

        htmlBody = htmlBody
            .Replace("{0}", EmailContents.Auth.ConfirmEmailOTP)
            .Replace("{1}", otp);

        await _emailService.SendEmailWithTemplateAsync(
            subject: "[EStore] OTP for Email Confirmation",
            mailTo: user.Email,
            htmlBody: htmlBody);
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
