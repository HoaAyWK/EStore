using ErrorOr;
using EStore.Application.Common.Dtos;
using EStore.Domain.CustomerAggregate;

namespace EStore.Application.Common.Interfaces.Services;

public interface IAuthenticationService
{
    Task<ErrorOr<AuthenticationResult>> LoginAsync(string email, string password);
    Task<ErrorOr<AuthenticationResult>> RegisterAsync(Customer user, string password);
    Task<ErrorOr<Success>> SendConfirmationEmailAddressEmailAsync(
        string email,
        string templatePath);

    Task<ErrorOr<Success>> VerifyEmailAsync(string email, string token);
    Task<ErrorOr<Success>> ForgetPasswordAsync(string email);
    Task<ErrorOr<Success>> ResetPasswordAsync(
        string email,
        string token,
        string password);
}
