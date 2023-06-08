using ErrorOr;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Category
    {
        public static Error NotFound = Error.NotFound(
            code: "Category.NotFound",
            description: "The category with specified identifier was not found.");

        public static Error ParentCategoryNotFound = Error.Validation(
            code: "Category.ParentCategoryNotFound",
            description: "The parent category with specified identifier was not found.");

        public static Error AlreadyContainedChildren = Error.Validation(
            code: "Category.AlreadyContainedChildren",
            description: "The category with specified identifier already contained children.");

        public static Error AlreadyContainedProducts = Error.Validation(
            code: "Category.AlreadyContainedProducts",
            description: "The category with specified identifier already contained products.");
    }
}
