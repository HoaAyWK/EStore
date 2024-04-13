using ErrorOr;
using EStore.Domain.BrandAggregate;
using EStore.Domain.Common.Errors;
using EStore.Domain.UnitTests.Brands.TestUtils;

namespace EStore.Domain.UnitTests.Brands;

public class UpdateBrandDetailsTests
{
    private readonly Brand _brand = Brand.Create(
        Constants.Brand.Name,
        string.Empty)
        .Value;

    [Theory]
    [ClassData(typeof(ValidBrandNameData))]
    public void UpdateBrandDetails_WhenValidName_ShouldUpdate(string name)
    {
        var result = _brand.UpdateDetails(name, string.Empty);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Updated);
    }

    [Theory]
    [ClassData(typeof(InvalidBrandNameData))]
    public void UpdateBrandName_WhenInvalidName_ShouldReturnInvalidNameLengthError(string name)
    {
        var result = _brand.UpdateDetails(name, string.Empty);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Brand.InvalidNameLength);
    }
}

