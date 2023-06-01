using ErrorOr;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class ProductAttribute
    {
        public static Error NotFound = Error.NotFound(
            code: "ProductAttribute.NotFound",
            description: "Product Attribute not found.");

        public static Error OptionSetNotFound = Error.NotFound(
            code: "ProductAttribute.OptionSetNotFound",
            description: "Product Attribute Option Set not found.");
    }
}
