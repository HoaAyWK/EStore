using ErrorOr;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error NotFound = Error.NotFound(
            code: "User.NotFound",
            description: "The user with specified identifier was not found.");

        public static Error DuplicateEmail = Error.Conflict(
            code: "User.DuplicateEmail",
            description: "User with given email already exists.");

        public static Error InvalidFirstNameLength = Error.Validation(
            code: "User.InvalidFirstNameLength",
            description: $"User first name must be between {Domain.UserAggregate.User.MinFirstNameLength} " +
                $"and {Domain.UserAggregate.User.MaxFirstNameLength} characters.");

        public static Error InvalidLastNameLength = Error.Validation(
            code: "User.InvalidLastNameLength",
            description: $"User last name must be between {Domain.UserAggregate.User.MinLastNameLength} " +
                $"and {Domain.UserAggregate.User.MaxLastNameLength} characters.");

        public static Error InvalidEmailLength = Error.Validation(
            code: "User.InvalidEmailLength",
            description: "User's email must be at least " +
                $"{Domain.UserAggregate.User.MinEmailLength} characters.");
    }
}
