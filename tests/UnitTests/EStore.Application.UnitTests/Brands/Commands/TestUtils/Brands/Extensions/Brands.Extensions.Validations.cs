using EStore.Domain.BrandAggregate;
using EStore.Application.Brands.Commands.CreateBrand;

namespace EStore.Application.UnitTests.Brands.Commands.TestUtils.Brands.Extensions;

public static partial class BrandExtensions
{
    public static void ValidateCreatedFrom(this Brand brand, CreateBrandCommand command)
    {
        brand.Name.Should().Be(command.Name);
    }
}