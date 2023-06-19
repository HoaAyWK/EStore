using EStore.Application.Brands.Commands.DeleteBrand;

namespace EStore.Api.UnitTests.Controllers.Brands.TestUtils;

public static class DeleteBrandCommandUtils
{
    public static DeleteBrandCommand Create()
        => new DeleteBrandCommand(Constants.Brand.Id);
}
