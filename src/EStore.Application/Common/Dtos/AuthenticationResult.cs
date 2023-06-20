using EStore.Domain.CustomerAggregate;

namespace EStore.Application.Common.Dtos;

public class AuthenticationResult
{
    public Customer Customer { get; init; } = null!;

    public string Token { get; init; } = null!;
}
