using ErrorOr;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Customer
    {
        public static Error NotFound = Error.NotFound(
            code: "Customer.NotFound",
            description: "The Customer with specified identifier was not found.");

        public static Error DuplicateEmail = Error.Conflict(
            code: "Customer.DuplicateEmail",
            description: "Customer with given email already exists.");

        public static Error InvalidFirstNameLength = Error.Validation(
            code: "Customer.InvalidFirstNameLength",
            description: $"Customer first name must be between {CustomerAggregate.Customer.MinFirstNameLength} " +
                $"and {CustomerAggregate.Customer.MaxFirstNameLength} characters.");

        public static Error InvalidLastNameLength = Error.Validation(
            code: "Customer.InvalidLastNameLength",
            description: $"Customer last name must be between {CustomerAggregate.Customer.MinLastNameLength} " +
                $"and {CustomerAggregate.Customer.MaxLastNameLength} characters.");
    }
}
