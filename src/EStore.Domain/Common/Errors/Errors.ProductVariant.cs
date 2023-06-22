using ErrorOr;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class ProductVariant
    {
        public static Error NotFound => Error.NotFound(
            code: "ProductVariant.NotFound",
            description: "Product variant with specified identifier was not found.");

        public static Error InvalidStockQuantity => Error.Validation(
            code: "ProductVariant.InvalidStockQuantity",
            description: "Stock quantity must be greater than or equal " +
                $"{Domain.ProductVariantAggregate.ProductVariant.MinStockQuantity}");

        public static Error InvalidPrice => Error.Validation(
            code: "ProductVariant.InvalidPrice",
            description: "Price must be greater than or equal " +
                $"{Domain.ProductVariantAggregate.ProductVariant.MinPrice}");

        public static Error ProductNotFound(ProductId productId)
            => Error.Validation(
                code: "ProductVariant.ProductNotFound",
                description: $"The product with id = {productId.Value} was not found.");

        public static Error DuplicateVariant => Error.Validation(
            code: "ProductVariant.DuplicateVariant",
            description: "Product variant already existed.");
    }
}
