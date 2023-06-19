using EStore.Application.Brands.Commands.UpdateBrand;
using EStore.Application.UnitTests.TestUtils.Constants;
using EStore.Domain.BrandAggregate.ValueObjects;

namespace EStore.Application.UnitTests.Brands.Commands.TestUtils;

public static class UpdateBrandCommandUtils
{
    public static UpdateBrandCommand CreateCommand(BrandId id, string? name = null)
        => new UpdateBrandCommand(id, name ?? Constants.Brand.NameForUpdating);
}
