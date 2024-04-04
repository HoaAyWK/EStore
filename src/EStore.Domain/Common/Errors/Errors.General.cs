using ErrorOr;
using EStore.Domain.Common.ValueObjects;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class General
    {
        public static Error UnProcessableRequest => Error.Failure(
            code: "General.UnProcessableRequest",
            description: "The server could not process the request.");

        public static Error InvalidStreetLength = Error.Validation(
            code: "Address.InvalidStreetLength",
            description: $"Street must be between {Address.MinStreetLength} " +
                $"and {Address.MaxStreetLength} characters.");

        public static Error InvalidCityLength = Error.Validation(
            code: "Address.InvalidCityLength",
            description: $"City must be between {Address.MinCityLength} " +
                $"and {Address.MaxCityLength} characters.");

        public static Error InvalidStateLength = Error.Validation(
            code: "Address.InvalidStateLength",
            description: $"State must be between {Address.MinStateLength} " +
                $"and {Address.MaxStateLength} characters.");

        public static Error InvalidCountryLength = Error.Validation(
            code: "Address.InvalidCountryLength",
            description: $"Country must be between {Address.MinCountryLength} " +
                $"and {Address.MaxCountryLength} characters.");
    }
}
