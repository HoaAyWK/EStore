using ErrorOr;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.Common.Errors;
using EStore.Domain.UnitTests.Categories.TestUtils;

namespace EStore.Domain.UnitTests.Categories;

public class UpdateCategoryDetailsTests
{
    private readonly Category _category = Category.Create(
        Constants.Category.Name,
        Constants.Category.Name,
        string.Empty,
        null)
        .Value;

    [Theory]
    [ClassData(typeof(ValidCategoryNameData))]
    public void UpdateCategoryDetails_WhenValidName_ShouldUpdate(string name)
    {
        var result = _category.UpdateDetails(name, string.Empty, name);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Updated);
    }

    [Theory]
    [ClassData(typeof(InvalidCategoryNameData))]
    public void UpdateCategoryDetails_WhenInvalidName_ShouldReturnInvalidNameLengthError(string name)
    {
        var result = _category.UpdateDetails(name, string.Empty, name);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Category.InvalidNameLength);
    }
}
