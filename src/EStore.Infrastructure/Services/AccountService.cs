using ErrorOr;
using EStore.Application.Common.Interfaces.Services;
using EStore.Contracts.Accounts;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using EStore.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using EStore.Infrastructure.Authentication;
using EStore.Contracts.Common;

namespace EStore.Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(
        ICustomerRepository customerRepository,
        UserManager<ApplicationUser> userManager)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
    }

    public async Task<ErrorOr<UserInfoResponse>> GetUserInfoAsync(CustomerId userId)
    {
        var user = await _customerRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return Errors.Account.NotFound;
        }

        var applicationUser = await _userManager.FindByIdAsync(userId.Value.ToString());

        if (applicationUser is null)
        {
            return Errors.Account.NotFound;
        }

        var roles = await _userManager.GetRolesAsync(applicationUser);
        var userRole = Roles.Customer;

        if (roles.Count > 0)
        {
            userRole = roles[0];
        }

        var addressResponses = user.Addresses.Select(address => new AddressResponse(
            Id: address.Id.Value,
            ReceiverName: address.ReceiverName,
            PhoneNumber: address.PhoneNumber,
            IsDefault: address.IsDefault,
            Street: address.Street,
            City: address.City,
            StateId: address.StateId,
            State: address.State,
            CountryId: address.CountryId,
            Country: address.Country,
            ZipCode: address.ZipCode))
            .ToList();
        
        return new UserInfoResponse(
            user.Id.Value,
            user.FirstName,
            user.LastName,
            user.Email,
            userRole,
            user.AvatarUrl,
            addressResponses);
    }
}
