using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Api.UnitTests.TestUtils.Constants;

public static partial class Constants
{
    public static class Category
    {
        public static readonly CategoryId Id = CategoryId.CreateUnique();

        public const string Name = "Category Name";

        public static readonly string NameUnderMinLength = new string('a', Domain.CategoryAggregate.Category.MinNameLength - 1);

        public const string InvalidParentId = "abc";
    }
}
