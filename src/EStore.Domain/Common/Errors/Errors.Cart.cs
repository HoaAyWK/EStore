using ErrorOr;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Cart
    {
        public static Error InvalidCartItemQuantity = Error.Validation(
            code: "Cart.InvalidCartItemQuantity",
            description: "Quantity of cart item must be greater than or equal " +
                $"{Domain.CartAggregate.Entities.CartItem.MinQuantity}.");

        public static Error InvalidCartItemUnitPrice = Error.Validation(
            code: "Cart.InvalidCartItemUnitPrice",
            description: "Unit price of cart item must be greater than or equal " +
                $"{Domain.CartAggregate.Entities.CartItem.MinUnitPrice}.");
    }
}
