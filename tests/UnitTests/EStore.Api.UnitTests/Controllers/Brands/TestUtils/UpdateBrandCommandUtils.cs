using EStore.Application.Brands.Commands.UpdateBrand;

namespace EStore.Api.UnitTests.Controllers.Brands.TestUtils;

public static class UpdateBrandCommandUtils
{
    public static UpdateBrandCommand Create()
        => new UpdateBrandCommand(Constants.Brand.Id, Constants.Brand.Name);
}
