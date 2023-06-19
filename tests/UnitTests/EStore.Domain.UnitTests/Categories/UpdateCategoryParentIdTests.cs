using EStore.Domain.CategoryAggregate;

namespace EStore.Domain.UnitTests.Categories;

public class UpdateCategoryParentIdTests
{
    Category category = Category.Create(Constants.Category.Name, null).Value;

    [Fact]
    public void UpdateCategoryParentId_WhenParentIdIsNotNull_ShouldUpdate()
    {
        category.UpdateParentCategory(Constants.Category.ParentCategoryId);

        category.ParentId.Should().Be(Constants.Category.ParentCategoryId);
    }

    [Fact]
    public void UpdateCategoryParentId_WhenParenIdIsNull_ShouldUpdate()
    {
        category.UpdateParentCategory(null);

        category.ParentId.Should().BeNull();
    }
}
