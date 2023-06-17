using ErrorOr;
using EStore.Application.Common.Dtos;
using EStore.Domain.UserAggregate;

namespace EStore.Application.Common.Interfaces.Services;

public interface IAuthenticationService
{
    Task<ErrorOr<AuthenticationResult>> LoginAsync(string email, string password);
    Task<ErrorOr<AuthenticationResult>> RegisterAsync(User user, string password);
}
