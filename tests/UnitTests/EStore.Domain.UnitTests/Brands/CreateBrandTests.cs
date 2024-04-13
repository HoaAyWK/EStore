using EStore.Domain.BrandAggregate;
using EStore.Domain.Common.Errors;
using EStore.Domain.UnitTests.Brands.TestUtils;

namespace EStore.Domain.UnitTests.Brands;

public class CreateBrandTests
{
    [Theory]
    [ClassData(typeof(ValidBrandNameData))]
    public void CreateBrand_WhenValidName_ShouldReturnBrand(string name)
    {  
        var result = Brand.Create(name, string.Empty);

        result.IsError.Should().BeFalse();
        result.Value.Name.Should().Be(name);
    }

    [Theory]
    [ClassData(typeof(InvalidBrandNameData))]
    public void CreateBrand_WhenInvalidName_ShouldReturnInvalidNameLengthError(string name)
    {
        var result = Brand.Create(name, string.Empty);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Brand.InvalidNameLength);
    }
}
