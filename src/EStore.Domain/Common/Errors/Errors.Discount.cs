using ErrorOr;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Discount
    {
        public static Error NotFound => Error.NotFound(
            code: "Discount.NotFound",
            description: "The discount with specified identifier was not found.");

        public static Error InvalidNameLength => Error.Validation(
            code: "Discount.InvalidNameLength",
            description: "Discount name must be between " +
                $"{Domain.DiscountAggregate.Discount.MinNameLength}" +
                $"{Domain.DiscountAggregate.Discount.MaxNameLength} characters.");

        public static Error InvalidDiscountPercentage => Error.Validation(
            code: "Discount.InvalidDiscountPercentage",
            description: "Discount Percentage must be between " +
                $"{Domain.DiscountAggregate.Discount.MinPercentage}" +
                $"{Domain.DiscountAggregate.Discount.MaxPercentage}.");

        public static Error InvalidDiscountAmount => Error.Validation(
            code: "Discount.InvalidDiscountAmount",
            description: "Discount Amount must be larger than or equal " +
                $"{Domain.DiscountAggregate.Discount.MinAmount}.");

        public static Error InvalidDiscountStartDate => Error.Validation(
            code: "Discount.InvalidDiscountStartDate",
            description: "Discount Start Date must be larger than current date.");

        public static Error InvalidDiscountEndDate => Error.Validation(
            code: "Discount.InvalidDiscountEndDate",
            description: "Discount End Date must be larger than current date.");

        public static Error InvalidDiscountDates => Error.Validation(
            code: "Discount.InvalidDiscountDates",
            description: "Discount Start Date must be larger than End Date.");
    }
}
