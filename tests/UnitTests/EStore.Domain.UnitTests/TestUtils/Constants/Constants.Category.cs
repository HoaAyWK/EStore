using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Domain.UnitTests.TestUtils.Constants;

public static partial class Constants
{
    public static class Category
    {
        public static readonly CategoryId ParentCategoryId = CategoryId.CreateUnique();

        public const string Name = "Category Name";

        public const string NameForUpdating = "Updated Category Name";

        public static readonly string NameHasMinLength = new string('a', Domain.CategoryAggregate.Category.MinNameLength);

        public static readonly string NameHasMaxLength = new string('a', Domain.CategoryAggregate.Category.MaxNameLength);

        public static readonly string NameUnderMinLength = new string('a', Domain.CategoryAggregate.Category.MinNameLength - 1);

        public static readonly string NameExceedMaxLength = new string('a', Domain.CategoryAggregate.Category.MaxNameLength + 1);
    }
}

