using EStore.Application.Brands.Commands.DeleteBrand;
using EStore.Application.UnitTests.TestUtils.Constants;
using EStore.Domain.BrandAggregate.ValueObjects;

namespace EStore.Application.UnitTests.Brands.Commands.TestUtils;

public static class DeleteBrandCommandUtils
{
    public static DeleteBrandCommand CreateCommand(BrandId? id = null)
        => new DeleteBrandCommand(id ?? Constants.Brand.MockExistingBrand.Id);
}
