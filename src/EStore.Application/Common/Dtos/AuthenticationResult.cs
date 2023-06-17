using EStore.Domain.UserAggregate;

namespace EStore.Application.Common.Dtos;

public class AuthenticationResult
{
    public User User { get; init; } = null!;

    public string Token { get; init; } = null!;
}
