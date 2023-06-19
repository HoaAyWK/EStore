using EStore.Domain.CategoryAggregate;
using EStore.Domain.Common.Errors;
using EStore.Domain.UnitTests.Categories.TestUtils;

namespace EStore.Domain.UnitTests.Categories;

public class CreateCategoryTests
{
    [Theory]
    [ClassData(typeof(ValidCategoryNameData))]
    public void CreateCategory_WhenValidName_ShouldReturnCategory(string name)
    {
        var result = Category.Create(name, Constants.Category.ParentCategoryId);

        result.IsError.Should().BeFalse();
        result.Value.Name.Should().Be(name);
        result.Value.ParentId.Should().Be(Constants.Category.ParentCategoryId);
    }

    [Theory]
    [ClassData(typeof(InvalidCategoryNameData))]
    public void CreateCategory_WhenInvalidName_ShouldReturnInvalidNameLengthError(string name)
    {
        var result = Category.Create(name, null);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Category.InvalidNameLength);
    }
}
