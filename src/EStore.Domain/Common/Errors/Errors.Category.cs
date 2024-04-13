using ErrorOr;
using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Category
    {
        public static Error NotFound = Error.NotFound(
            code: "Category.NotFound",
            description: "The category with specified identifier was not found.");

        public static Error NotFoundValidation(CategoryId id)
        {
            return Error.Validation(
                code: "Category.NotFoundValidation",
                description: $"The category with id = {id.Value} was not found.");
        }

        public static Error ParentCategoryNotFound = Error.Validation(
            code: "Category.ParentCategoryNotFound",
            description: "The parent category with specified identifier was not found.");

        public static Error AlreadyContainedChildren = Error.Validation(
            code: "Category.AlreadyContainedChildren",
            description: "The category with specified identifier already contained children.");

        public static Error AlreadyContainedProducts = Error.Validation(
            code: "Category.AlreadyContainedProducts",
            description: "The category with specified identifier already contained products.");

        public static Error InvalidNameLength = Error.Validation(
            code: "Category.InvalidNameLength",
            description: "Category name must be between " +
                $"{CategoryAggregate.Category.MinNameLength} and " +
                $"{CategoryAggregate.Category.MaxNameLength} characters.");

        public static Error CategoryAlreadyExists(string name)
        {
            return Error.Validation(
                code: "Category.CategoryAlreadyExists",
                description: $"The category with name = {name} already exists.");
        }

        public static Error InvalidSlugLength = Error.Validation(
            code: "Category.InvalidSlugLength",
            description: "Category slug must be between " +
                $"{CategoryAggregate.Category.MinSlugLength} and " +
                $"{CategoryAggregate.Category.MaxSlugLength} characters.");

        public static Error CategoryWithProvidedSlugAlreadyExists(string slug)
        {
            return Error.Validation(
                code: "Category.CategoryWithProvidedSlugAlreadyExists",
                description: $"The category with slug = {slug} already exists.");
        }
    }
}
