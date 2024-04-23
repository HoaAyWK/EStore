using ErrorOr;
using EStore.Domain.CustomerAggregate.Entities;

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

        public static Error InvalidPhoneNumberLength = Error.Validation(
            code: "Customer.InvalidPhoneLength",
            description: $"Phone number must be between {CustomerAggregate.Customer.MinPhoneNumberLength} " +
                $"and {CustomerAggregate.Customer.MaxPhoneNumberLength} characters.");

        public static Error PhoneNumberAlreadyExists = Error.Conflict(
            code: "Customer.PhoneNumberAlreadyExists",
            description: "Customer with given phone number already exists.");

        public static Error InvalidStreetLength = Error.Validation(
            code: "Customer.InvalidStreetLength",
            description: $"Street must be between {Address.MinStreetLength} " +
                $"and {Address.MaxStreetLength} characters.");

        public static Error InvalidCityLength = Error.Validation(
            code: "Customer.InvalidCityLength",
            description: $"City must be between {Address.MinCityLength} " +
                $"and {Address.MaxCityLength} characters.");

        public static Error InvalidStateLength = Error.Validation(
            code: "Customer.InvalidStateLength",
            description: $"State must be between {Address.MinStateLength} " +
                $"and {Address.MaxStateLength} characters.");

        public static Error InvalidCountryLength = Error.Validation(
            code: "Customer.InvalidCountryLength",
            description: $"Country must be between {Address.MinCountryLength} " +
                $"and {Address.MaxCountryLength} characters.");

        public static Error InvalidZipCodeLength = Error.Validation(
            code: "Customer.InvalidZipCodeLength",
            description: $"ZipCode must be between {Address.MinZipCodeLength} " +
                $"and {Address.MaxZipCodeLength} characters.");

        public static Error AddressNotFound = Error.NotFound(
            code: "Customer.AddressNotFound",
            description: $"The address with specified identifier was not found.");

        public static Error InvalidReceiverNameLength = Error.Validation(
            code: "Customer.InvalidReceiverNameLength",
            description: $"Receiver name must be between {Address.MinReceiverNameLength} " +
                $"and {Address.MaxReceiverNameLength} characters.");
    }
}
