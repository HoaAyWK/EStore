using ErrorOr;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.ValueObjects;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Cart
    {
        public static Error NotFound => Error.NotFound(
            code: "Cart.NotFound",
            description: "The cart with specified identifier was not found.");

        public static Error Existing(CustomerId customerId) =>
            Error.Validation(
                code: $"The cart with customer id = {customerId.Value} already existed.");

        public static Error ProductNotFound(ProductId productId)
            => Error.Validation(
                code: "Cart.ProductNotFound",
                description: $"The product with id = {productId.Value} was not found.");

        public static Error ProductVariantNotFound(ProductVariantId productVariantId)
            => Error.Validation(
                code: "Cart.ProductVariantNotFound",
                description: $"The product variant with id = {productVariantId.Value} was not found.");

        public static Error InvalidProductVariant => Error.Validation(
            code: "Cart.InvalidProductVariant",
            description: "Product variant id must not be empty for the product including product variants.");
            
        public static Error ProductNotHadVariant(ProductId productId, ProductVariantId productVariantId)
            => Error.Validation(
                code: "Cart.ProductNotHadVariant",
                description: $"The product with id = {productId.Value} did not have " +
                    $"the product variant with id = {productVariantId.Value}.");

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
