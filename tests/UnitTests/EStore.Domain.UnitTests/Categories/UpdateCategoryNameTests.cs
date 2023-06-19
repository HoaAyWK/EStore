using ErrorOr;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.Common.Errors;
using EStore.Domain.UnitTests.Categories.TestUtils;

namespace EStore.Domain.UnitTests.Categories;

public class UpdateCategoryNameTests
{
    Category category = Category.Create(Constants.Category.Name, null).Value;

    [Theory]
    [ClassData(typeof(ValidCategoryNameData))]
    public void UpdateCategoryName_WhenValidName_ShouldUpdate(string name)
    {
        var result = category.UpdateName(name);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Updated);
    }

    [Theory]
    [ClassData(typeof(InvalidCategoryNameData))]
    public void UpdateCategoryName_WhenInvalidName_ShouldReturnInvalidNameLengthError(string name)
    {
        var result = category.UpdateName(name);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Category.InvalidNameLength);
    }
}
