using System.Text;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Customers.Events.CustomerCreated;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace EStore.Infrastructure.IntegrationEvents.Customers.CustomerCreated;

public sealed class CustomerCreatedIntegrationEventHandler : INotificationHandler<CustomerCreatedIntegrationEvent>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;

    public CustomerCreatedIntegrationEventHandler(
        ICustomerRepository customerRepository,
        UserManager<ApplicationUser> userManager,
        IEmailService emailService)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
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

        string emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
        string token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationToken));

        await _emailService.SendEmailAsync("Confirm your email", user.Email, token);
    }
}
