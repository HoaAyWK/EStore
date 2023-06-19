using EStore.Application.Brands.Commands.CreateBrand;

namespace EStore.Api.UnitTests.Controllers.Brands.TestUtils;

public static class CreateBrandCommandUtils
{
    public static CreateBrandCommand Create(string name)
        => new CreateBrandCommand(name);
}
