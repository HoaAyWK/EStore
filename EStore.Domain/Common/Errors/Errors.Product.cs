using ErrorOr;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Product
    {
        public static Error NotFound = Error.NotFound(
            code: "Product.NotFound",
            description: "Product not found.");

        public static Error UnprovidedSpecialPriceStartDate = Error.Validation(
            code: "Product.UnprovidedSpecialPriceStartDate",
            description: "Start date is required when Special price is provided.");

        public static Error UnprovidedSpecialPriceEndDate = Error.Validation(
            code: "Product.UnprovidedSpecialPriceEndDate",
            description: "End date is required when Special price is provided.");

        public static Error SpecialPriceStartDateLessThanEndDate = Error.Validation(
            code: "Product.SpecialPriceStartDateLessThanEndDate",
            description: "End date must be lagger than Start date.");
    }
}
