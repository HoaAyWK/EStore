using ErrorOr;
using EStore.Domain.BrandAggregate;
using EStore.Domain.Common.Errors;
using EStore.Domain.UnitTests.Brands.TestUtils;

namespace EStore.Domain.UnitTests.Brands;

public class UpdateBrandNameTests
{
    Brand brand = Brand.Create(Constants.Brand.Name).Value;

    [Theory]
    [ClassData(typeof(ValidBrandNameData))]
    public void UpdateBrandName_WhenValidName_ShouldUpdate(string name)
    {
        var result = brand.UpdateName(name);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Updated);
    }

    [Theory]
    [ClassData(typeof(InvalidBrandNameData))]
    public void UpdateBrandName_WhenInvalidName_ShouldReturnInvalidNameLengthError(string name)
    {
        var result = brand.UpdateName(name);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Brand.InvalidNameLength);
    }
}

