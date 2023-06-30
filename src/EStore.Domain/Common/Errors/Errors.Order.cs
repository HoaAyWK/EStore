using ErrorOr;
using EStore.Domain.CartAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.ValueObjects;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Order
    {
        public static Error CartNotFound = Error.Validation(
                code: "Order.CartNotFound",
                description: $"The cart with specified identifier was not found.");

        public static Error CartIsEmpty = Error.Validation(
            code: "Order.CartIsEmpty",
            description: "Your cart is empty.");
    }
}
