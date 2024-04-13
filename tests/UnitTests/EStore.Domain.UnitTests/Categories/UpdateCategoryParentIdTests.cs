using EStore.Domain.CategoryAggregate;

namespace EStore.Domain.UnitTests.Categories;

public class UpdateCategoryParentIdTests
{
    private readonly Category _category = Category.Create(
        Constants.Category.Name,
        Constants.Category.Name,
        string.Empty,
        null)
        .Value;

    [Fact]
    public void UpdateCategoryParentId_WhenParentIdIsNotNull_ShouldUpdate()
    {
        _category.UpdateParentCategory(Constants.Category.ParentCategoryId);

        _category.ParentId.Should().Be(Constants.Category.ParentCategoryId);
    }

    [Fact]
    public void UpdateCategoryParentId_WhenParenIdIsNull_ShouldUpdate()
    {
        _category.UpdateParentCategory(null);

        _category.ParentId.Should().BeNull();
    }
}
