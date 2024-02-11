using EStore.Application.Common.Interfaces.Services;
using EStore.Infrastructure.Identity.Enums;

namespace EStore.Infrastructure.Identity;

public class AccountToken
{
    public Guid Id { get; set; }

    public string Email { get; set; } = default!;

    public string Token { get; set; } = default!;

    public TokenType TokenType { get; set; }

    public DateTime ExpireDate { get; set; }

    private AccountToken()
    {
    }

    public AccountToken(
        string email,
        string token,
        TokenType tokenType,
        DateTime expireDate)
    {
        Email = email;
        Token = token;
        TokenType = tokenType;
        ExpireDate = expireDate;
    }

    public bool IsExpired(IDateTimeProvider dateTimeProvider)
    {
        return ExpireDate < dateTimeProvider.UtcNow;
    }
}
