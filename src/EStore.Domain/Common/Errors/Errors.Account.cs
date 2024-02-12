using ErrorOr;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Account
    {
        public static Error NotFound => Error.Validation(
            code: "Account.NotFound",
            description: "Account not found.");
    }
}
