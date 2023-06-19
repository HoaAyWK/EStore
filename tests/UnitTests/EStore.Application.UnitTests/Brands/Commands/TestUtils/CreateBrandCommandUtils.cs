using EStore.Application.Brands.Commands.CreateBrand;
using EStore.Application.UnitTests.TestUtils.Constants;

namespace EStore.Application.UnitTests.Brands.Commands.TestUtils;

public static class CreateBrandCommandUtils
{
    public static CreateBrandCommand CreateCommand(string? name = null)
        => new CreateBrandCommand(name ?? Constants.Brand.Name);
}
