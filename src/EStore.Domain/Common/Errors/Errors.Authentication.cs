using ErrorOr;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials = Error.Validation(
            code: "Auth.InvalidCredentials",
            description: "Invalid credentials.");

        public static Error Unauthenticated = Error.Validation(
            code: "Aut.Unauthenticated",
            description: "You are not logged in.");
    }
}
