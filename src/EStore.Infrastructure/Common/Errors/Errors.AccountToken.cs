using ErrorOr;

namespace EStore.Infrastructure.Common.Errors;

public static class AccountTokenErrors
{
    public static Error InvalidAccountToken => Error.Validation(
        code: "AccountToken.Invalid",
        description: "Invalid OTP or token.");

    public static Error TokenExpired => Error.Validation(
        code: "AccountToken.TokenExpired",
        description: "Token already expired.");
}
