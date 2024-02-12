using ErrorOr;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error NotFound => Error.Validation(
            code: "User.NotFound",
            description: "User not found.");
    }
}
