using ErrorOr;
using EStore.Domain.BrandAggregate.ValueObjects;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Brand
    {
        public static Error NotFound = Error.NotFound(
            code: "Brand.NotFound",
            description: "Brand not found.");

        public static Error AlreadyContainedProducts = Error.Validation(
            code: "Brand.AlreadyContainedProducts",
            description: "The brand with specified identifier already contained products.");

        public static Error NotFoundValidation(BrandId id)
        {
            return Error.Validation(
                code: "Brand.NotFound",
                description: $"The brand with id = {id.Value} was not found.");
        }

        public static Error InvalidNameLength = Error.Validation(
            code: "Brand.InvalidNameLength",
            description: "Brand name must be between " +
                $"{BrandAggregate.Brand.MinNameLength} and " +
                $"{BrandAggregate.Brand.MaxNameLength} characters.");

        public static Error BrandAlreadyExists(string name)
        {
            return Error.Validation(
                code: "Brand.BrandAlreadyExists",
                description: $"The brand with name = {name} was not found.");
        }
    }
}
