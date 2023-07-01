using ErrorOr;
using EStore.Domain.OrderAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.ValueObjects;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Order
    {
        public static Error NotFound = Error.NotFound(
            code: "Order.NotFound",
            description: "The order with specified identifier was not found.");

        public static Error CartNotFound = Error.Validation(
                code: "Order.CartNotFound",
                description: $"The cart with specified identifier was not found.");

        public static Error CartIsEmpty = Error.Validation(
            code: "Order.CartIsEmpty",
            description: "Your cart is empty.");

        public static Error CannotUpdate(OrderId id)
            => Error.Validation(
                code: "Order.CannotUpdate",
                description: $"Can not update the order with id = {id.Value} because it was cancelled.");

        public static Error ProductNotFound(ProductId productId)
            => Error.Validation(
                code: "Order.ProductNotFound",
                description: $"The product with id ={productId.Value} was not found.");

        public static Error ProductVariantNotFound(ProductVariantId productVariantId)
            => Error.Validation(
                code: "Order.ProductVariantNotFound",
                description: $"The product variant with id = {productVariantId.Value} was not found.");

        public static Error UnpaidOrder(OrderId id)
            => Error.Validation(
                code: "Order.UnpaidOrder",
                description: $"The order with id = {id.Value} was unpaying.");
    }
}
