using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Application.UnitTests.TestUtils.Constants;

public static partial class Constants
{
    public static class Category
    {
        public static readonly CategoryId Id = CategoryId.CreateUnique();

        public static readonly CategoryId OtherId = CategoryId.CreateUnique();
        
        public const string Name = "Category Name";

        public const string NameForUpdating = "Category Name Updated";

        public static readonly CategoryId ParentId = CategoryId.CreateUnique();

        public static readonly string NameHasMinLength = new string ('a', Domain.CategoryAggregate.Category.MinNameLength);

        public static readonly string NameHasMaxLength = new string('a', Domain.CategoryAggregate.Category.MaxNameLength);

        public static readonly string NameUnderMinLength = new string('a', Domain.CategoryAggregate.Category.MinNameLength - 1);

        public static readonly string NameExceedMaxLength = new string('a', Domain.CategoryAggregate.Category.MaxNameLength + 1);

        public static readonly Domain.CategoryAggregate.Category MockExistingCategoryNoParent =
            Domain.CategoryAggregate.Category.Create(Name, Name, string.Empty, null).Value;

        public static readonly Domain.CategoryAggregate.Category MockExistingCategoryHasParent =
            Domain.CategoryAggregate.Category.Create(Name, Name, string.Empty, ParentId).Value;

        public static readonly Domain.CategoryAggregate.Category MockExistingParentCategory =
            Domain.CategoryAggregate.Category.Create(Name, Name, string.Empty, null).Value;
    }
}
