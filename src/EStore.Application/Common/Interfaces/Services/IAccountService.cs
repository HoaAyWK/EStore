using ErrorOr;
using EStore.Contracts.Accounts;
using EStore.Domain.CustomerAggregate.ValueObjects;

namespace EStore.Application.Common.Interfaces.Services;

public interface IAccountService
{
    Task<ErrorOr<UserInfoResponse>> GetUserInfoAsync(CustomerId userId);
}
